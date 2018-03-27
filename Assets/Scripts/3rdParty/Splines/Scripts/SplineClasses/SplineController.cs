using Se;
using UnityEngine;
using System.Collections;

/*
 * http://www.opensource.org/licenses/lgpl-2.1.php
 * Copyright Defective Studios 2009-2011
 */
///<author>Matt Schoen</author>
///<date>5/21/2011</date>
///<email>schoen@defectivestudios.com</email>
///<version>10</version>
/// <summary>
/// Spline-based motion controller
/// </summary>

[RequireComponent(typeof(Rigidbody))]
public class SplineController : MonoBehaviour {

	public float RotationDamping = 0.5f;
	public float InitialRotationSpeed = 0.3f;
	public float SnapDistance = 1;
	public float RotationSpeedMax = 15;
	public float Speed = 2.0f;
	public int PredictionCount = 2;
	public float PredictionCoef = 1.0f;
	public float PlayerBarycenter = 0.51f;
	public float Height = 1;

	private float currentSpeed;

	public Spline CurrentSpline;
	public SplineNode CurrentNode;

	private float rotationTime = 0;
	private float currentRotationSpeed = 0.1f;

	private Vector3 targetUp;
	private Vector3 targetForward;

	private bool turnBack = false;

	public static int SplineTopLayer = 31;

	public virtual void Start() {
		if(CurrentSpline) {
			CurrentSpline.follower = this;
		}
	}

	void updateTarget(float input)
	{
		Rigidbody body = GetComponent<Rigidbody>();
		SplineNode next;
		Vector3 velocity, position;
		if (FindNextSplineNode(out next, out velocity, out position))
		{
			rotationTime = 0.2f;
			currentRotationSpeed = InitialRotationSpeed*2;
			transform.position = position;
			body.velocity = body.velocity.magnitude * Mathf.Sign(Vector3.Dot(body.velocity, next.forward)) * next.forward;
		}
		CurrentNode = next;
		if (CurrentNode && !turnBack)
		{
			if (body.velocity.sqrMagnitude < 0.1)
			{
				currentSpeed = 0;
				targetForward = (CurrentNode.forward * Mathf.Sign(Vector3.Dot(transform.forward, CurrentNode.forward)));
			}
			else
			{
				currentSpeed = Vector3.Dot(body.velocity, CurrentNode.forward);
				targetForward = (CurrentNode.forward * Mathf.Sign(currentSpeed));
			}
			targetUp = CurrentNode.up;

			if (input > 0)
			{
				if (currentSpeed == 0)
				{
					rotationTime = 0;
					currentRotationSpeed = InitialRotationSpeed;
				}
			}
			else if (input == 0)
			{
				rotationTime = 0;
				currentRotationSpeed = InitialRotationSpeed;
			}
		}
	}

	void updateVelocity(float input)
	{
		if (input > 0)
		{
			if (CurrentNode != null)
			{
				Rigidbody body = GetComponent<Rigidbody>();
				float projectionMagnitude = Vector3.Dot(body.velocity, targetForward);
				if (projectionMagnitude * projectionMagnitude < 0.5 * body.velocity.sqrMagnitude) // straight rotation
				{
					rotationTime = 0.2f;
					currentRotationSpeed = InitialRotationSpeed * 2;
					Vector3 currentVelocity = body.velocity;
					body.velocity = Vector3.zero;
					body.AddForce(currentVelocity.magnitude * targetForward, ForceMode.Impulse);
				}
				else
				{
					Vector3 acc = targetForward * CurrentNode.acceleration * Speed;
					body.AddForce(acc * Time.fixedDeltaTime, ForceMode.Impulse);
					if (body.velocity.sqrMagnitude > CurrentNode.speed * CurrentNode.speed)
					{
						body.velocity = body.velocity.normalized * CurrentNode.speed;
					}
				}
			}

		}
		else
		{
			Rigidbody body = GetComponent<Rigidbody>();
			if (CurrentNode != null)
			{
				body.velocity = Vector3.Dot(body.velocity.normalized, targetForward) * body.velocity.magnitude * targetForward;
			}
			body.velocity *= 0.8f;
		}

	}

	void updateRotation(float input)
	{
		if (CurrentNode)
		{
			if (turnBack)
			{
				turnBack = Vector3.Dot(transform.forward, targetForward) < 0.99;
			}
			else if (input < 0)
			{
				turnBack = true;
				rotationTime = 0;
				currentRotationSpeed = InitialRotationSpeed * 1.5f;
				targetForward = -1 * (CurrentNode.forward * Mathf.Sign(Vector3.Dot(transform.forward, CurrentNode.forward)));
			}
			rotationTime = Mathf.SmoothDamp(rotationTime, 1, ref currentRotationSpeed,
												RotationDamping, RotationSpeedMax, Time.fixedDeltaTime);
			transform.rotation = Quaternion.Lerp(transform.rotation,
																					Quaternion.LookRotation(targetForward, targetUp),
																					rotationTime);
		}
	}

	void Drive(float input = 0) {
		if (CurrentNode)
		{
			updateTarget(input);
			updateVelocity(input);
			updateRotation(input);
		}
		else
		{
			Spline nextSpline;
			SplineNode nextNode;
			Vector3 position, currentPosition = transform.position -
				transform.up * Height * PlayerBarycenter;
			if (FindNextSpline(currentPosition, GetComponent<Rigidbody>().velocity*Time.fixedDeltaTime*PredictionCoef, 
												out position, out nextSpline, out nextNode))
			{
					Land(position, nextSpline, nextNode);
			}
		}
	}

	public virtual void FixedUpdate() {
		CharacterController character = GetComponent<CharacterController>();

		if (character)
			Drive(Se.InputActions.MovementDirection.y);
		else
			Drive();
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.cyan;
		if(CurrentNode) {
			Gizmos.DrawCube(CurrentNode.transform.position, Vector3.one * 0.1f);
			Gizmos.DrawLine(CurrentNode.transform.position, CurrentNode.transform.position + CurrentNode.forward);
			Gizmos.DrawLine(transform.position, transform.position + transform.forward);
			Gizmos.color = Color.magenta;
			Gizmos.DrawLine(CurrentNode.transform.position, CurrentNode.transform.position + CurrentNode.up);
			Gizmos.DrawLine(transform.position, transform.position + transform.up);
		}
		if(CurrentSpline)
		{
			for (uint i=0; i<CurrentSpline.length;++i)
			{
				Gizmos.color = Color.blue;
				Gizmos.DrawLine(CurrentSpline[i].transform.position, CurrentSpline[i].transform.position + CurrentSpline[i].forward);
				Gizmos.color = Color.red;
				Gizmos.DrawLine(CurrentSpline[i].transform.position, CurrentSpline[i].transform.position + CurrentSpline[i].up);
			}
		}
	}

	public void Detach() {
		CharacterController ctrl = GetComponent<CharacterController>();
		Rigidbody body = GetComponent<Rigidbody>();
		if (CurrentSpline.next)
		{
			if (CurrentSpline.next.begin)
			{
				CurrentSpline = CurrentSpline.next;
				CurrentNode = CurrentSpline.begin;
				SplineNode next;
				Vector3 position, velocity;
				if (FindNextSplineNode(CurrentNode, out next, out velocity, out position))
				{
					PartialLand(position, velocity, next);
					return;
				}
			}
		}
		if (CurrentSpline.previous)
		{
			if (CurrentSpline.previous.end)
			{
				CurrentSpline = CurrentSpline.previous;
				CurrentNode = CurrentSpline.end;
				SplineNode next;
				Vector3 position, velocity;
				if (FindNextSplineNode(CurrentNode, out next, out velocity, out position))
				{
					PartialLand(position, velocity, next);
					return;
				}
			}
		}
		Debug.Log("detach" + Time.frameCount);
		body.useGravity = true;
		Hero heroScript = GetComponent<Hero>();
		if (heroScript)
			heroScript.enabled = true;
		body.AddForce(ctrl.velocity.magnitude * (targetForward-targetUp) * 5.0f, ForceMode.Impulse);
		CurrentSpline.Oust();
		CurrentSpline = null;
		CurrentNode = null;
	}

	bool FindNextSplineNode(out SplineNode next, out Vector3 velocity, out Vector3 position)
	{
		if (CurrentNode)
		{
			Rigidbody body = GetComponent<Rigidbody>();
			if (FindNextSplineNode(CurrentNode, out next, out velocity, out position))
			{
				if (CurrentNode != next)
				{
					return true;
				}
				return false;
			}
			Detach();
		}
		next = null;
		velocity = Vector3.zero;
		position = Vector3.zero;
		return false;
	}

	bool FindNextSplineNode(SplineNode current, out SplineNode next, out Vector3 velocity, out Vector3 position)
	{
		Vector3 currentPosition = transform.position - transform.up * Height * PlayerBarycenter;
		Vector3 newPosition = currentPosition;
		next = current;
		float currentSnapDist = SnapDistance;
		Rigidbody body = GetComponent<Rigidbody>();
		float predictionCoef = PredictionCoef;
		int i;
		for (i = 0; i < PredictionCount; ++i)
		{
			if (FindSplineVertex(CurrentSpline, currentPosition, 
												currentPosition + body.velocity * Time.fixedDeltaTime * predictionCoef,
												current, out next, out newPosition, 3))
				break;
			predictionCoef *= 0.8f;
			SnapDistance *= 1.3f;
		}
		SnapDistance = currentSnapDist;
		position = newPosition + transform.up * Height * PlayerBarycenter;
		if (i < PredictionCount)
		{
			velocity = body.velocity * predictionCoef;
			return true;
		}
		velocity = body.velocity;
		return false;
	}

	public bool FindSplineVertex(Spline s, Vector3 a, Vector3 b, out SplineNode vert, out Vector3 position) {
		return FindSplineVertex(s, a, b, null, out vert, out position);
	}

	bool checkSplineNode(SplineNode node, Vector3 a, Vector3 b, float sqrSnapDistance,
											out Vector3 position)
	{
		if (node != null && node.next != null)
		{
			Vector3 PosToA = b - node.transform.position;
			float dot = Vector3.Dot(PosToA, node.forward);
			if (dot >= 0)
			{
				Vector3 projection = node.transform.position + dot * node.forward;
				Debug.DrawLine(projection, b, Color.green);

				if ((b - projection).sqrMagnitude < sqrSnapDistance
					&& (node.transform.position - projection).sqrMagnitude < node.sqrMagnitude)
				{
					position = projection;
					return true;
				}
			}
		}
		position = Vector3.zero;
		return false;
	}

	bool FindSplineVertex(Spline s, Vector3 a, Vector3 b, SplineNode guess,
												out SplineNode vert, out Vector3 position, int count=-1) {
		SplineNode countup, countdown;
		if (s != null) {
			if(guess == null) {
				if(!(s.begin && s.end)) {
					Debug.LogError("Could not find vertex, begin and end not set");
					vert = null;
					position = Vector3.zero;
					return false;
				}
				countup = s.begin;
				countdown = s.end;
			} else {
				countdown = guess;
				countup = guess;
			}
			float squareSnap = SnapDistance * SnapDistance;
			do
			{
				if (countup)
				{
					if (checkSplineNode(countup, a, b, squareSnap, out position))
					{
						vert = countup;
						return true;
					}
					countup = countup.next;
				}
				if (countdown)
				{
					if (checkSplineNode(countdown.previous, a, b, squareSnap, out position))
					{
						vert = countdown.previous;
						return true;
					}
					countdown = countdown.previous;
				}
				count--;
			} while (countdown != countup && count!=0);
		}
		vert = null;
		position = Vector3.zero;
		return false;
	}

	bool FindNextSpline(Vector3 position, Vector3 velocity, 
											out Vector3 landingPos, out Spline nextSpline, out SplineNode nextNode) {
		RaycastHit hit;
		Debug.DrawLine(position, position+velocity, Color.magenta);
		
		LayerMask mask = LayerMask.GetMask(LayerMask.LayerToName(SplineTopLayer));
		if (Physics.Linecast(position, position+velocity, out hit, mask.value))
		{
			if ((hit.point - position).sqrMagnitude < SnapDistance * SnapDistance)
			{
				if (hit.transform.parent)
				{
					nextSpline = hit.transform.parent.GetComponent<Spline>();
					SplineCollider collider = hit.transform.GetComponent<SplineCollider>();
					nextNode = null;
					if (collider)
						nextNode = collider.node;

					if (FindSplineVertex(nextSpline, position, position + velocity, nextNode, out nextNode, out landingPos))
						return true;

					Debug.LogWarning("Error in findNextSpline - Couldn't Land");
				}
			}
		}
		nextNode = null;
		nextSpline = null;
		landingPos = Vector3.zero;
		return false;
	}

	public void Land(Vector3 position, Spline landingSpline, SplineNode landingNode) {
		Debug.Log("attach" + Time.frameCount);
		Rigidbody body = GetComponent<Rigidbody>();
		body.useGravity = false;
		Hero heroScript = GetComponent<Hero>();
		if (heroScript)
			heroScript.enabled = false;
		rotationTime = 0; currentRotationSpeed = InitialRotationSpeed;
		transform.position = position + transform.up * Height * PlayerBarycenter;
		CurrentSpline = landingSpline;
		CurrentNode = landingNode;
		body.velocity *= 0.8f;
		updateVelocity(0.5f);
	}

	public void PartialLand(Vector3 position, Vector3 velocity, SplineNode landingNode)
	{
		Debug.Log("reattach" + Time.frameCount);
		Rigidbody body = GetComponent<Rigidbody>();
		body.useGravity = false;
		body.velocity = body.velocity * 0.5f;
		transform.position = position;
		CurrentNode = landingNode;
	}
}