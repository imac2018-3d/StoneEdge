using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargetTest : MonoBehaviour {
	public float movementSpeed = 10;
	public float turningSpeed = 10;

	private Transform lastTransform;

	// Use this for initialization
	void Start () {
		
	}

	void Update() {
		float horizontal = Input.GetAxis("Horizontal") * turningSpeed;
		GetComponent<Rigidbody> ().AddRelativeTorque (new Vector3 (0, horizontal, 0));

		float vertical = Input.GetAxis("Vertical") * movementSpeed;
		//transform.Translate(0, 0, vertical);
		GetComponent<Rigidbody> ().AddRelativeForce (new Vector3 (0, 0, vertical));
		//GetComponent<Rigidbody>().AddRelativeForce
	}
}
