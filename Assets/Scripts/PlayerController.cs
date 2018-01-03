using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private Rigidbody rb;
	private Vector3 newPos;

	void Start () {
		rb = GetComponent<Rigidbody> ();
	}

	void FixedUpdate() {
		rb.constraints = RigidbodyConstraints.FreezeRotation;

		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal/2.0f, 0.0f, moveVertical/2.0f);

		rb.MovePosition (transform.position + movement);
	}
}
