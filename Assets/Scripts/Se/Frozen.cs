using UnityEngine;
using Utils;

namespace Se {

	public static class GameObjectFreezeExtensions {
		/// <summary>Freeze this GameObject (see the Frozen component class).</summary>
		public static void Freeze(this GameObject go) {
			if (!go.IsFrozen())
				go.AddComponent<Frozen>();
		}
		/// <summary>De-freeze this GameObject (see the Frozen component class).</summary>
		public static void Defreeze(this GameObject go) {
			go.GetComponent<Frozen>().MapNotNull(frozenComponent => GameObject.Destroy(frozenComponent));
		}
		/// <summary>Is this GameObject frozen ?</summary>
		public static bool IsFrozen(this GameObject go) {
			return go.GetComponent<Frozen>() != null;
		}
	}

	// NOTE: Right now, this only freezes Rigidbodies.
	// TODO: Freeze any Animation-related component;
	// TODO: Use SetActive(false) on other relevant components;
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
			savedRigidbody = GetComponent<Rigidbody> ();
			savedRigidbody.MapNotNull (rb => {
				savedRigidbodyState.Velocity = rb.velocity;
				savedRigidbodyState.AngularVelocity = rb.angularVelocity;
			});
		}
		void freeze() {
			savedRigidbody.MapNotNull (rb => {
				// NOTE: rb.Sleep() alone is not enough.
				rb.velocity = Vector3.zero;
				rb.angularVelocity = Vector3.zero;
				rb.Sleep();
			});
		}
		void restore() {
			savedRigidbody.MapNotNull (rb => {
				rb.WakeUp();
				rb.velocity = savedRigidbodyState.Velocity;
				rb.angularVelocity = savedRigidbodyState.AngularVelocity;
			});
		}
	}
}