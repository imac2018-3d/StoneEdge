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

	private float currentSpeed;
	private float currentAcceleration;

	public Spline CurrentSpline;
	public SplineNode GroundNode;

	private float rotationT = 0;
	private float currentRotationSpeed = 0.1f;

	private Vector3 targetRotationUp;
	private Vector3 targetRotationForward;

	public static int SplineTopLayer = 31;

	void OnGUI() {
		GUI.Box(new Rect(Screen.width - 75, 25, 75, 20), GetComponent<Rigidbody>().velocity.ToString());
	}
	public virtual void Start() {
		if(CurrentSpline) {
			CurrentSpline.followers.Add(this);
		}
	}

	void Drive(float input = 0) {
		Rigidbody body = GetComponent<Rigidbody>();
		if (GroundNode)
		{
			input = 1;
			body.angularVelocity = Vector3.zero;
			currentSpeed = Vector3.Dot(body.velocity, GroundNode.forward);
			if (input != 0)
			{
				if ((currentSpeed==0 ^ input==0) || Mathf.Sign(input) != Mathf.Sign(currentSpeed))
				{
					rotationT = 0;
					currentRotationSpeed = InitialRotationSpeed;
				}
				targetRotationForward = Mathf.Sign(input) * GroundNode.forward;
				targetRotationUp = GroundNode.up;
			}
			else
			{
				if (Mathf.Abs(currentSpeed) >= 0.5)
				{
					rotationT = 0;
					currentRotationSpeed = InitialRotationSpeed;
				}
				targetRotationForward = transform.forward;
				targetRotationUp = GroundNode.up;
			}
			if (Mathf.Abs(currentSpeed+GroundNode.acceleration*input*Speed) < GroundNode.speed)
				currentAcceleration = input*GroundNode.acceleration * Speed;
			else
				currentAcceleration = (GroundNode.speed-currentSpeed) / Time.deltaTime;
			body.AddForce(currentAcceleration * Time.deltaTime * GroundNode.forward, ForceMode.Impulse);
			rotationT = Mathf.SmoothDamp(rotationT, 1, ref currentRotationSpeed, RotationDamping, RotationSpeedMax, Time.deltaTime);
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetRotationForward, targetRotationUp),rotationT);
			FindNextSplineNode();
		}
		else
		{
			bool landed;
			int traveltime;
			Vector3 position, currentPosition = transform.position - transform.up * GetComponent<CapsuleCollider>().height * 0.52f;
			if (FindNextSpline(currentPosition, GetComponent<Rigidbody>().velocity*Time.deltaTime*PredictionCoef,
													out position, out landed, out traveltime))
			{
				if (landed)
				{
					Land(position);
				}
			}
		}
	}

	public virtual void Update() {
		CharacterControl character = GetComponent<CharacterControl>();

		if (character)
			Drive(character.MoveZ);
		else
			Drive();
	}

	void OnDrawGizmos() {
		//Ground = red
		//Grav = green
		//Other = blue
		Gizmos.color = Color.cyan;
		if(GroundNode) {
			Gizmos.DrawCube(GroundNode.transform.position, Vector3.one * 0.1f);
			Gizmos.DrawLine(GroundNode.transform.position, GroundNode.transform.position + GroundNode.forward);
			Gizmos.DrawLine(transform.position, transform.position + transform.forward);
			Gizmos.color = Color.magenta;
			Gizmos.DrawLine(GroundNode.transform.position, GroundNode.transform.position + GroundNode.up);
			Gizmos.DrawLine(transform.position, transform.position + transform.up);
		}
		if(CurrentSpline)
		{
			for (int i=0; i<CurrentSpline.length;++i)
			{
				Gizmos.color = Color.blue;
				Gizmos.DrawLine(CurrentSpline[i].transform.position, CurrentSpline[i].transform.position + CurrentSpline[i].forward);
				Gizmos.color = Color.red;
				Gizmos.DrawLine(CurrentSpline[i].transform.position, CurrentSpline[i].transform.position + CurrentSpline[i].up);
			}
		}
	}
	//	public void UpdateSpline(){
	//		if(CurrentSpline != CurrentSplineSource.spline){
	//			Svertex oldBegin = CurrentSpline.begin;
	//			Svertex newBegin = CurrentSplineSource.spline.begin;
	//			while(oldBegin.next != null && newBegin.next != null){		//roll forward through both splines until we find the old groundCV
	//				if(oldBegin == groundCV)
	//					break;
	//				oldBegin = oldBegin.next;
	//				newBegin = newBegin.next;
	//			}
	//			groundCV = newBegin;
	//			CurrentSpline = CurrentSplineSource.spline;
	//		}
	//	}

	public void Detach() {
		Rigidbody body = GetComponent<Rigidbody>();
		if (CurrentSpline.next)
		{
			if (CurrentSpline.next.begin)
			{
				CurrentSpline = CurrentSpline.next;
				GroundNode = CurrentSpline.begin;
				SplineNode next;
				Vector3 velocity, position;
				if (FindNextSplineNode(GroundNode, out next, out velocity, out position))
				{
					body.velocity = velocity;
					transform.position = position;
					return;
				}
			}
		}
		if (CurrentSpline.previous)
		{
			if (CurrentSpline.previous.end)
			{
				CurrentSpline = CurrentSpline.previous;
				GroundNode = CurrentSpline.end;
				SplineNode next;
				Vector3 velocity, position;
				if (FindNextSplineNode(GroundNode, out next, out velocity, out position))
				{
					body.velocity = velocity;
					transform.position = position;
					return;
				}
			}
		}
		Debug.Log("detach" + Time.frameCount);
		GetComponent<Rigidbody>().useGravity = true;
		CurrentSpline = null;
		GroundNode = null;
	}

	bool FindNextSplineNode()
	{
		if (GroundNode)
		{
			SplineNode next;
			Rigidbody body = GetComponent<Rigidbody>();
			Vector3 velocity, position;
			if (FindNextSplineNode(GroundNode, out next, out velocity, out position))
			{
				if (GroundNode != next)
				{
					transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetRotationForward, targetRotationUp), 0.2f);
					rotationT = 0; currentRotationSpeed = InitialRotationSpeed;
					GroundNode = next;
				}
				body.velocity = velocity;
				transform.position = position;
				return true;
			}
			Detach();
		}
		return false;
	}

	bool FindNextSplineNode(SplineNode current, out SplineNode next, out Vector3 velocity, out Vector3 position)
	{
		Vector3 currentPosition = transform.position - transform.up * GetComponent<CapsuleCollider>().height * 0.52f;
		Vector3 newPosition = currentPosition;
		next = current;
		float currentSnapDist = SnapDistance;
		Rigidbody body = GetComponent<Rigidbody>();
		float predictionCoef = PredictionCoef;
		int i;
		for (i = 0; i < PredictionCount; ++i)
		{
			if (FindSplineVertex(CurrentSpline, currentPosition, currentPosition + body.velocity * Time.deltaTime * predictionCoef,
												current, out next, out newPosition, 3))
				break;
			predictionCoef *= 0.8f;
			SnapDistance *= 1.3f;
		}
		SnapDistance = currentSnapDist;
		position = newPosition + GroundNode.up * GetComponent<CapsuleCollider>().height * 0.52f;
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

	bool checkSpineNode(SplineNode node, Vector3 a, Vector3 b, float sqrSnapDistance,
											out Vector3 position)
	{
		if (node != null && node.next != null)
		{
			Vector3 PosToA = b - node.transform.position;
			float dot = Vector3.Dot(PosToA, node.forward);
			if (dot >= 0)
			{
				Vector3 projection = node.transform.position + dot * node.forward;
				//Utility.Vector3Pair points =
				//Utility.Dist3DSegToSeg(a, b, node.transform.position, node.next.transform.position);
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
					if (checkSpineNode(countup, a, b, squareSnap, out position))
					{
						vert = countup;
						return true;
					}
					countup = countup.next;
				}
				if (countdown)
				{
					if (checkSpineNode(countdown.previous, a, b, squareSnap, out position))
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
											out Vector3 landing, out bool landed, out int traveltime) {
		traveltime = 0;
		RaycastHit hit;
		bool found = false;
		Debug.DrawLine(position, position+velocity, Color.magenta);
		LayerMask mask = LayerMask.GetMask(LayerMask.LayerToName(SplineTopLayer));
		if (Physics.Linecast(position, position+velocity, out hit, mask.value))
		{
			found = true;
		}
		if(found && (hit.point - position).sqrMagnitude < SnapDistance*SnapDistance) {   //This means that the first cast hit the collider
			if(hit.transform.parent)
				CurrentSpline = hit.transform.parent.GetComponent<Spline>();
			else
				CurrentSpline = hit.transform.GetComponent<Spline>();
			if(!CurrentSpline)
				CurrentSpline = hit.transform.GetComponentInChildren<Spline>();
			if(CurrentSpline) {
				CurrentSpline.followers.Add(this);
				//transform.position = hit.point;
				transform.parent = CurrentSpline.transform;
				if(FindSplineVertex(CurrentSpline, position, position + velocity, out GroundNode, out landing))
				{
					landed = true;
				} else {
					landed = false;
					Debug.LogWarning("Error in findNextSpline - Couldn't Land");
				}
				return true;
			}
		}
		landing = Vector3.zero;
		landed = false;
		return false;
	}

	public bool FindNextSplineMouse(out Vector3 position, Vector3 point) {
		RaycastHit hit;
		if(Physics.Raycast(point, Vector3.down, out hit, SnapDistance, 1 << SplineTopLayer)) {
			if(hit.transform.parent.GetComponent<Spline>()) {
				CurrentSpline = hit.transform.parent.GetComponent<Spline>();
				if(FindSplineVertex(CurrentSpline, point, point + Vector3.down * SnapDistance,
														out GroundNode, out position)) {
					return true;
				}
			}
		}
		position = Vector3.zero;
		return false;
	}
	//-------------------//
	//	SPLINE PHYSICS!!!//
	//-------------------//
	bool NextNode() {
		if(GroundNode.next) {
			GroundNode = GroundNode.next;
			return true;
		}
		else if(CurrentSpline.next) {
			if(CurrentSpline.next.begin) {
				CurrentSpline = CurrentSpline.next;
				GroundNode = CurrentSpline.begin;
				return true;
			} else Debug.LogWarning("Next spline missing begin");
		}
		return false;
	}
	bool PreviousNode() {
		if(GroundNode.previous) {
			GroundNode = GroundNode.previous;
			return true;
		}
		else {
			if(CurrentSpline.previous) {
				if(CurrentSpline.previous.end) {
					CurrentSpline = CurrentSpline.previous;
					GroundNode = CurrentSpline.end;
					return true;
				} else Debug.LogWarning("Previous spline missing end");
			}
		}
		return false;
	}
	public virtual void Land(Vector3 position) {
		Rigidbody body = GetComponent<Rigidbody>();
		body.useGravity = false;
		body.velocity = Vector3.zero;
		rotationT = 0; currentRotationSpeed = InitialRotationSpeed;
		transform.position = position + GroundNode.up * GetComponent<CapsuleCollider>().height * 0.52f;
	}
}