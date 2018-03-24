using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Se {
		public class FollowingCamera : MonoBehaviour {

		// Plan:
		// - Le saut ne doit pas bouger la CameraTarget
		//   Faire un lookat progressif.
		// - Il faut avoir un DesiredSelfToTarget.
		//   C'est celui qu'on transforme. Le SelfToTarget ne fait que s'en rapprocher progressivement.

		[Tooltip("GameObject to follow.")]
		public Transform Target;
		public float RotationAroundYSpeed;
		public float RotationAroundXSpeed;

		public float AngleAroundX;
		public float AngleAroundY;
		public float Distance;

		void Awake () {
			Assert.IsNotNull (Target);
		}

		void LateUpdate() { // NOTE: Not Update(), because otherwise there's stuttering.
			var input = InputActions.CameraMovementDirection;
			AngleAroundY += input.x * Time.deltaTime * RotationAroundYSpeed;
			AngleAroundX += input.y * Time.deltaTime * RotationAroundXSpeed;

			AngleAroundX = Mathf.Clamp (AngleAroundX, 0f, 80f);
			AngleAroundY = Mathf.Repeat (AngleAroundY, 360f);

			Distance = computeDistanceFromAngleAroundX ();

			var ry = Quaternion.AngleAxis(AngleAroundY, Vector3.up);
			var rx = Quaternion.AngleAxis(AngleAroundX, Vector3.right);
			var selfToTarget = ry * rx * Vector3.forward * Distance;
			transform.position = Target.position - selfToTarget;
			transform.LookAt (Target.position);
		}

		float computeDistanceFromAngleAroundX() {
			return 20f * Mathf.Clamp(AngleAroundX, 10f, 50f) / 70f;
		}
	}

/*
	public class FollowingCamera : MonoBehaviour {

		// Plan:
		// - Le saut ne doit pas bouger la CameraTarget
		//   Faire un lookat progressif.
		// - Il faut avoir un DesiredSelfToTarget.
		//   C'est celui qu'on transforme. Le SelfToTarget ne fait que s'en rapprocher progressivement.

		[Tooltip("GameObject to follow.")]
		public Transform Target;
		public float MinDistanceFromTarget = 2f;
		public float MaxDistanceFromTarget = 8f;
		public float YRotationSpeedFactor = 1f;
		public float XRotationSpeedFactor = 1f;
		public float AltitudeHack = 3f;
		public bool WantsToCaptureCursor;
		public bool EnableAverageAltitudeTrick;

		Vector3 desiredSelfToTarget;

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

			desiredSelfToTarget = new Vector3 (0, 0, -MaxDistanceFromTarget);
			var desiredPosition = Target.position - desiredSelfToTarget.normalized * MaxDistanceFromTarget;

			if (EnableAverageAltitudeTrick) {
				var avg = float.NaN; // averageAltitude (4);
				if (float.IsNaN (avg) || float.IsInfinity (avg)) {
					Debug.LogWarning ("averageAltitude() returned " + avg + "!");
				} else {
					desiredPosition += Vector3.up * ((Target.transform.position.y + AltitudeHack) - (transform.position.y - avg));
				}
			
			}
			transform.position = Vector3.Lerp (transform.position, desiredPosition, 0.1f);

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
*/
} // namespace Se