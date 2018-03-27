using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projection : MonoBehaviour {

	public bool Project = false;
	public ForceMode mode = ForceMode.Force;

	// Update is called once per frame
	void Update () {
		if (Project)
		{
			Project = false;
			Rigidbody body = GetComponent<Rigidbody>();
			body.AddForce(new Vector3(1, 1, 0), mode);
		}
	}
}
