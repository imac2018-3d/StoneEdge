using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private Rigidbody rb;  // containt the hero's informations
	private Vector3 newPos;
	public bool playerEnabledToMove;

	void Start () {
		playerEnabledToMove = true;
		rb = GetComponent<Rigidbody> ();
	}

	void FixedUpdate() {
		rb.constraints = RigidbodyConstraints.FreezeRotation;  // Used to avoid the character to fall down in an uphill

		float moveHorizontal = Input.GetAxis ("Horizontal");  // Get the horizontal keyboard arrows "forces"
		float moveVertical = Input.GetAxis ("Vertical");  // Get the vertical keyboard arrows "forces"

		Vector3 movement = new Vector3 (moveHorizontal/2.0f, 0.0f, moveVertical/2.0f);  // Create a vector to move the character 

		if(playerEnabledToMove)
			rb.MovePosition (transform.position + movement);  // Move the character
	}
}
