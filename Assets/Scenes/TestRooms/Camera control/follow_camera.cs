using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class follow_camera : MonoBehaviour {
	public GameObject target;
	public float altitude = 4;

	private float distance;

	// Use this for initialization
	void Start () {
		distance = (target.transform.position - transform.position).magnitude;
	}

	// computes average altitude on multiples raycasts (casted from the line between avatar and camera)
	private float averageAltitude(int ray_number) {
		int number = 0;
		float mean_altitude = 0;
		Vector3 cam_to_avatar = target.transform.position - transform.position;
		RaycastHit hit;
		for (int i = 0; i < ray_number; ++i) {
			Vector3 origin = transform.position + i * cam_to_avatar / ray_number;
			if (Physics.Raycast (origin, new Vector3 (0, -1, 0), out hit, 5)) {
				Debug.DrawRay (origin, hit.point - origin, Color.red);
				number += 1;
				mean_altitude += hit.point.y;
			}
		}
		return mean_altitude / number;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 vecTargetToCamNormalized = (transform.position - target.transform.position).normalized;
		transform.position = target.transform.position + distance * vecTargetToCamNormalized;

		// adjust camera altitude
		transform.Translate(new Vector3(0, altitude - (transform.position.y - averageAltitude(4)), 0));


		// at the very end, we adjust camera orientation
		transform.LookAt (target.transform);
	}
}
