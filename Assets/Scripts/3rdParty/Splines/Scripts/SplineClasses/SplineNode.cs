using UnityEngine;
using System.Collections;

/*
 * http://www.opensource.org/licenses/lgpl-2.1.php
 * Spline Tester for Defective Spline
 * Copyright Defective Studios 2009-2011
 */
[ExecuteInEditMode]
public class SplineNode : MonoBehaviour {
	public int index;
	public float colliderRadius = 0.125f;
	public float addOffset = .5f;

	public float acceleration = 0.5f;
	public float speed = 10;

	public SplineNode next, previous;
	public Collider spanCollider;
	public Spline spline;
	public bool destroyed;

	Vector3 _up = Vector3.up;
	public Vector3 up
	{
		get { return transform.parent.rotation*_up; }
		set { _up = value.normalized; }
	}

	Vector3 _forward = Vector3.right;
	public Vector3 forward {
		get {
			return _forward;
		}
	}
	float _sqrMagnitude = 0;
	public float sqrMagnitude
	{
		get
		{
			return _sqrMagnitude;
		}
	}

	public Vector3 toNext
	{
		get
		{
			if (next != null)
				return next.transform.position - transform.position;
			else
				return Vector3.right;
		}
	}

	public void updateForward()
	{
		CapsuleCollider collider = GetComponent<CapsuleCollider>();
		if (collider)
			collider.height = toNext.magnitude;
		_forward = toNext.normalized;
		_sqrMagnitude = toNext.sqrMagnitude;
	}

	public Vector3 getForward(bool ahead) {
		if(ahead)
			return forward;
		else if(previous != null)
			return previous.forward;
		else {
			Debug.Log("Error in SplineNode - getForward() - No previous vert");
			return Vector3.right;
		}
	}

	public static SplineNode Create() {
		GameObject obj = new GameObject();
		SplineNode node = obj.AddComponent<SplineNode>();
		obj.transform.Rotate(Vector3.up, 90);
		return node;
	}

	void Start() {
		if(!spline)
			if(transform.parent)
				spline = transform.parent.GetComponent<Spline>();

		RefreshModel();
		ReOrient();
	}

	public void ReOrient() {
		if (next)
		{
			transform.LookAt(next.transform, transform.up);
			CapsuleCollider cap = (CapsuleCollider)spanCollider;

			if (cap)
			{
				cap.transform.position = transform.position + toNext * 0.5f;
				cap.transform.LookAt(next.transform, transform.up);
				cap.radius = colliderRadius * toNext.magnitude * 0.5f;
				cap.height = toNext.magnitude;
			}
		}
	}

	public void RefreshModel() {
	}

	public void AddCollider() {
		if(spanCollider)
			DestroyImmediate(spanCollider.gameObject);
		if(next) {
			GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Capsule);
			spanCollider = obj.GetComponent<Collider>();
			CapsuleCollider cap = obj.GetComponent<CapsuleCollider>();
			cap.isTrigger = true;
			cap.direction = 2;
			cap.height = toNext.magnitude;
			cap.radius = colliderRadius * toNext.magnitude * 0.5f;
		
			spanCollider.gameObject.AddComponent<SplineCollider>();
			spanCollider.GetComponent<SplineCollider>().node = this;
			spanCollider.GetComponent<Renderer>().enabled = false;
			spanCollider.transform.parent = transform.parent;
			
			if(transform.parent)
			if(transform.parent.GetComponent<Spline>())
				if(transform.parent.GetComponent<Spline>().playerWalkable)
					spanCollider.gameObject.layer = SplineController.SplineTopLayer;
			spanCollider.isTrigger = true;
		}
	}

	public GameObject AddNext() {
		//New Node
		GameObject dup = (GameObject)GameObject.Instantiate(gameObject);
		dup.SetActive(false);
		dup.SetActive(true);
		if(transform.parent)
			dup.transform.parent = transform.parent;
		dup.transform.position = transform.position + Vector3.right * addOffset;
		dup.transform.rotation = Quaternion.FromToRotation(Vector3.forward, Vector3.right);
		dup.transform.localScale = transform.localScale;
		dup.name = "Node";
		SplineNode dupNode = dup.GetComponent<SplineNode>();
		dupNode.spanCollider = null;
		if(next) {
			next.previous = dupNode;
			dupNode.next = next;
			dupNode.AddCollider();
		}	
		next = dupNode;													//The dup is now my previous node
		dupNode.previous = this;										//I'm the dup's next node
		AddCollider();
		if(spline)
			spline.AddVert(dupNode);
		return dup;
	}
	public GameObject AddPrev() {
		GameObject dup = (GameObject)GameObject.Instantiate(gameObject);			//Start by copying me
		//For some reason, all children are active on duplication.  We just need to deactivate all of the children, and reactivate the parent
		dup.SetActive(false);
		dup.SetActive(true);
		if(transform.parent)
			dup.transform.parent = transform.parent;						//If I have a parent, add it to the new node
		dup.transform.position = transform.position + Vector3.left * addOffset;
		dup.transform.localScale = transform.localScale;
		dup.name = "Node";
		SplineNode dupNode = dup.GetComponent<SplineNode>();
		dupNode.spanCollider = null;
		if(dupNode) {
			if(previous) {
				previous.next = dupNode;									//My previous node now points to the new node
				dupNode.previous = previous;								//The new node points back to my old previous
			}
			previous = dupNode;												//The dup is now my previous node
			dupNode.next = this;											//I'm the dup's next node
		}
		dupNode.AddCollider();
		if(spline)
			spline.AddVert(dupNode);
		return dup;
	}

	public void Disconnect() {
		if(next) {
			if(spanCollider) {
				DestroyImmediate(spanCollider.gameObject);
				spanCollider = null;
			}
			if(previous) {
				next.previous = previous;
				previous.AddCollider();
			} else next.previous = null;
		}
		if(previous) {
			if(next) {
				previous.next = next;
				previous.AddCollider();
			} else {
				previous.next = null;
				if(previous.spanCollider) {
					DestroyImmediate(previous.spanCollider.gameObject);
				}
				previous.spanCollider = null;
			}
		}
		if(spline) {
			if(this == spline.end)
				spline.end = previous;
			if(this == spline.begin)
				spline.begin = next;
		}
	}
	//This introduced a very interesting bug... It seems the entire scene is destoyed and re-instantiated on running the game.
	void OnDestroy() {
		if(!destroyed)
			Disconnect();
	}
}
