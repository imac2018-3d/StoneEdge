using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

using Se;

namespace Se {
	public class FollowingCamera : MonoBehaviour {

		[Tooltip("GameObject to follow.")]
		public Transform Target;
		public float MinDistanceFromTarget = 2f;
		public float MaxDistanceFromTarget = 8f;
		public float YRotationSpeedFactor = 1f;
		public float XRotationSpeedFactor = 1f;
		public float AltitudeHack = 3f;
		public bool WantsToCaptureCursor;

		void Awake () {
			Assert.IsNotNull (Target);
		}

		public Vector3 SelfToTarget {
			get { return Target.position - transform.position; }
			set {
				transform.position = Target.position - value;
				transform.LookAt (Target.position);
			}
		}
		public float DistanceFromTarget {
			get { return SelfToTarget.magnitude; }
			set { SelfToTarget = SelfToTarget.normalized * Mathf.Clamp(value, MinDistanceFromTarget, MaxDistanceFromTarget); }
		}

		void LateUpdate() { // NOTE: Not Update(), because otherwise there's stuttering.
			Cursor.lockState = WantsToCaptureCursor ? CursorLockMode.Locked : CursorLockMode.None;
			Cursor.visible = !WantsToCaptureCursor;

			var distanceHack = MaxDistanceFromTarget;
			var desired = Target.position - SelfToTarget.normalized * distanceHack;
			var avg = float.NaN; // averageAltitude (4);
			if (float.IsNaN (avg) || float.IsInfinity (avg)) {
				Debug.LogWarning ("averageAltitude() returned " + avg + "!");
			} else {
				desired += Vector3.up * ((Target.transform.position.y + AltitudeHack) - (transform.position.y - avg));
			}
			transform.position = Vector3.Lerp (transform.position, desired, 0.05f);
			var input = InputActions.CameraMovementDirection;
			SelfToTarget = Quaternion.AngleAxis (input.x * Time.deltaTime * YRotationSpeedFactor, Vector3.up) * SelfToTarget;
			SelfToTarget = Quaternion.AngleAxis (input.y * Time.deltaTime * XRotationSpeedFactor, transform.right) * SelfToTarget;
		}

		// Computes average altitude using multiples raycasts (from the line between target and camera)
		float averageAltitude(int nbRays) {
			int total = 0;
			float altitudesSum = 0;
			RaycastHit hit;
			for (int i = 0; i < nbRays; ++i) {
				var origin = transform.position + i * SelfToTarget / nbRays;
				if (Physics.Raycast (origin, Vector3.down, out hit, 5)) {
					Debug.DrawRay (origin, hit.point - origin, Color.red);
					total += 1;
					altitudesSum += hit.point.y;
				}
			}
			return altitudesSum / (float) total;
		}
	}
} // namespace Se