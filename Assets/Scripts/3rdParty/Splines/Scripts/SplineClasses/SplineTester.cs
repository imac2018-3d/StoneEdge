using UnityEngine;
using System.Collections;

/*
 * http://www.opensource.org/licenses/lgpl-2.1.php
 * Copyright Defective Studios 2011
 */
///<author>Matt Schoen</author>
///<date>5/21/2011</date>
///<email>schoen@defectivestudios.com</email>
/// <summary>
/// Spline Tester for Defective Spline
/// </summary>

[RequireComponent(typeof(Rigidbody))]
public class SplineTester : SplineController {

	public float initSpeed;
	public float accelScale;
	public Transform accelDirection;

	void Awake() {
		GetComponent<Rigidbody>().velocity = transform.forward * initSpeed;
	}
	void OnDrawGizmos() {
	}
}
