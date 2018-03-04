using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyPreventFalling : MonoBehaviour {

	private Rigidbody rb;  // containt the hero's informations

	void Start () {
		rb = GetComponent<Rigidbody> ();
	}

	void FixedUpdate() {
		rb.constraints = RigidbodyConstraints.FreezeRotation;  // Used to avoid the character to fall down in an uphill
	}
}
