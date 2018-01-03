// A component for testing the Frozen component and associated extension methods.
//
// A GameObject that has a FrozenTest component may be frozen and de-frozen
// by pressing either the Space or Escape key.

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace Se {
	public class FrozenTest : MonoBehaviour {
		private KeyCode[] keys = new KeyCode[]{ KeyCode.Escape, KeyCode.Space };

		void Start() {
			keys.AssertNotNull ();
			Debug.Log("Freeze this GameObject by pressing either Space or Escape!");
		}

		// Qui se souvient du WEI
		void Update() {
			if (keys.Any(Input.GetKeyDown)) {
				if (gameObject.IsFrozen()) {
					Debug.Log ("Defreeze!");
					gameObject.Defreeze();
				} else {
					Debug.Log ("Freeze!");
					gameObject.Freeze();
				}
			}
		}
	}
}