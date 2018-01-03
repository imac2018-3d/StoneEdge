using UnityEngine;
using Utils;
using UnityEngine.Timeline;

namespace Se {

	public static class GameObjectFreezeExtensions {
		/// <summary>Freeze this GameObject (see the Frozen component class).</summary>
		public static void Freeze(this GameObject go) {
			if (!go.IsFrozen())
				go.AddComponent<Frozen>();
		}
		/// <summary>De-freeze this GameObject (see the Frozen component class).</summary>
		public static void Defreeze(this GameObject go) {
			go.GetComponent<Frozen>().IfValidComponent(frozenComponent => GameObject.Destroy(frozenComponent));
		}
		/// <summary>Is this GameObject frozen ?</summary>
		public static bool IsFrozen(this GameObject go) {
			return go.GetComponent<Frozen>() != null;
		}
	}

	// TODO: Use SetActive(false/true) on other relevant components (e.g Hero, Enemy, etc) when we add them;
	public class Frozen : MonoBehaviour {

		struct RigidbodyState {
			public Vector3 Velocity, AngularVelocity;
		}
		RigidbodyState savedRigidbodyState = new RigidbodyState {
			Velocity = Vector3.zero,
			AngularVelocity = Vector3.zero,
		};
		// NOTE: There's a deprecated property named 'rigidbody' - avoid conflicts
		Rigidbody savedRigidbody;
		Animation savedAnimation;
		AudioSource savedAudioSource;
		Vector3 savedLocalPosition, savedLocalScale;
		Quaternion savedLocalRotation;

		void Awake () {
			save ();
			freeze ();
		}
		void Update () {
			freeze (); // Keep frozen every frame!
		}
		void OnDestroy() {
			restore ();
		}

		void save() {
			savedLocalPosition = gameObject.transform.localPosition;
			savedLocalRotation = gameObject.transform.localRotation;
			savedLocalScale = gameObject.transform.localScale;
			savedRigidbody = GetComponent<Rigidbody> ();
			savedRigidbody.IfValidComponent (rb => {
				savedRigidbodyState.Velocity = rb.velocity;
				savedRigidbodyState.AngularVelocity = rb.angularVelocity;
			});
			savedAnimation = GetComponent<Animation> ();
			savedAudioSource = GetComponent<AudioSource> ();
		}
		void freeze() {
			gameObject.transform.localPosition = savedLocalPosition;
			gameObject.transform.localRotation = savedLocalRotation;
			gameObject.transform.localScale = savedLocalScale;
			savedRigidbody.IfValidComponent (rb => {
				// NOTE: rb.Sleep() alone is not enough.
				rb.velocity = Vector3.zero;
				rb.angularVelocity = Vector3.zero;
				rb.Sleep();
			});
			// Pausing currently playing animation: https://answers.unity.com/answers/354415/view.html
			savedAnimation.IfValidComponent(a => a.enabled = false);
			savedAudioSource.IfValidComponent (a => a.Pause());
		}
		void restore() {
			gameObject.transform.localPosition = savedLocalPosition;
			gameObject.transform.localRotation = savedLocalRotation;
			gameObject.transform.localScale = savedLocalScale;
			savedRigidbody.IfValidComponent (rb => {
				rb.WakeUp();
				rb.velocity = savedRigidbodyState.Velocity;
				rb.angularVelocity = savedRigidbodyState.AngularVelocity;
			});
			savedAnimation.IfValidComponent(a => a.enabled = true);
			savedAudioSource.IfValidComponent (a => a.UnPause());
		}
	}
}