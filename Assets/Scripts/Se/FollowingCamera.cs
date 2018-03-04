using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class FollowingCamera : MonoBehaviour {
	public GameObject Target;
	public float Altitude = 4;

	float distance;

	public Vector3 SelfToTargetVector {
		get { return Target.transform.position - transform.position; }
	}

	void Start () {
		Assert.IsNotNull (Target);
		distance = SelfToTargetVector.magnitude;
	}

	void Update () {
		transform.position = Target.transform.position + distance * (-SelfToTargetVector.normalized);

		var avg = averageAltitude (4);
		if (!(float.IsNaN (avg) || float.IsInfinity (avg))) {
			transform.Translate (Vector3.up * (Altitude - (transform.position.y - avg)));
		} else {
			Debug.LogWarning ("averageAltitude() returned " + avg + "!");
		}

		transform.LookAt (Target.transform);
	}

	// Computes average altitude on multiples raycasts (casted from the line between avatar and camera)
	float averageAltitude(int nbRays) {
		int total = 0;
		float altitudesSum = 0;
		RaycastHit hit;
		for (int i = 0; i < nbRays; ++i) {
			var origin = transform.position + i * SelfToTargetVector / nbRays;
			if (Physics.Raycast (origin, Vector3.down, out hit, 5)) {
				Debug.DrawRay (origin, hit.point - origin, Color.red);
				total += 1;
				altitudesSum += hit.point.y;
			}
		}
		Assert.AreNotApproximatelyEqual (total, 0);
		return altitudesSum / total;
	}
}
