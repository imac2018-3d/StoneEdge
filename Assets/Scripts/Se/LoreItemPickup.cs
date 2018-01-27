using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Se {
	[RequireComponent(typeof(Collider))]
	public class LoreItemPickup : UniqueID<LoreItemPickup> {
		void Start() {
			Assert.IsTrue (gameObject.isStatic, "\"" + gameObject.name + "\": "+GetType().Name+"s should be static! Please check the \"Static\" box for this object in the inspector.");
			Assert.IsTrue (GetComponent<Collider>().isTrigger, "\"" + gameObject.name + "\": "+GetType().Name+" Colliders should be triggers! Please check the \"Is Trigger\" box in the inspector.");
		}
		void OnTriggerEnter(Collider cld) {
			var hero = cld.gameObject.GetComponent<Hero> ();
			if (hero == null)
				return;
			// Adding to a HashSet is fine because the key is always only present once.
			Assert.AreEqual (CurrentGameSaveData.Data.LoreItems.GetType(), typeof(HashSet<int>));
			bool wasAlreadyAcquired = !CurrentGameSaveData.Data.LoreItems.Add (ID);
			Debug.Log ("Acquired Lore Item \""+gameObject.name+"\" (ID:"+ID+")" + (wasAlreadyAcquired ? " (was already acquired)" : ""));
			// NOTE: For now, don't destroy the object. Pretend that the guardian has memorized the text.
		}
	}
}
