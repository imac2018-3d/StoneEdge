using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Se {
	[RequireComponent(typeof(Collider))]
    public class Checkpoint : MonoBehaviour {
		public int ID;

		// Used to ensure uniqueness of IDs.
		static Dictionary<int, Checkpoint> AllCheckpoints = new Dictionary<int, Checkpoint>();

		public void Start() {
			Assert.IsTrue (ID > 0, "\""+gameObject.name+"\": Checkpoint IDs must have a unique value! 0 or below is invalid.");

			if (AllCheckpoints.Count == 0) { // Do this only once at the start of the game
				var allCheckpoints = FindObjectsOfType<Checkpoint>();
				foreach (var c in allCheckpoints) {
					Assert.IsFalse(AllCheckpoints.ContainsKey(c.ID),
						"\"" + c.gameObject.name + "\": Checkpoint ID " + c.ID
						+ " is already taken by \""+AllCheckpoints[c.ID].gameObject.name+"\"! You must choose another."
					);
				}
			}
		}
    }
}
