using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour {

	private Rigidbody rb;
	private Vector3 jumpMovement;
	private bool grounded;

	void Start () {
		rb = GetComponent<Rigidbody> ();
	}

	// Detect collision with floor
	void OnCollisionEnter(Collision hit)
	{
		if (hit.gameObject.name == "Ground") {
			grounded = true;
		}
	}

	void OnCollisionExit(Collision hit)
	{
		if (hit.gameObject.name == "Ground") {
			grounded = false;
		}
	}
		
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space) && grounded == true) {
			jumpMovement = new Vector3 (0.0f, 7.0f, 0.0f);
			rb.AddForce(jumpMovement, ForceMode.Impulse);
		}
	}
}
