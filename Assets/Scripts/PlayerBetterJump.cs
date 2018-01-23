using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBetterJump : MonoBehaviour {

	public float fallMultiplier = 2.5f; // the multiplier to make the falling down part quicker than the jumping part
	public float lowJumpMultiplier = 2f; // The multiplier to make the jumping part lower than the fall down

	Rigidbody rb;

	void Start () {
		rb = GetComponent<Rigidbody> ();
	}

		// Apply the physics with the multopliers
	void Update () {
		if (rb.velocity.y < 0) {
			rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
		} else if (rb.velocity.y > 0 && !Input.GetKeyDown (KeyCode.Space)) {
			rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
		}
	}
}
