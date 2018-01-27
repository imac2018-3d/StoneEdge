using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System;

namespace Se {
	[RequireComponent(typeof(Collider))]
	public class Checkpoint : UniqueID<Checkpoint> {
		void Start() {
			Assert.IsTrue (gameObject.isStatic, "\"" + gameObject.name + "\": "+GetType().Name+"s should be static! Please check the \"Static\" box for this object in the inspector.");
			Assert.IsTrue (GetComponent<Collider>().isTrigger, "\"" + gameObject.name + "\": "+GetType().Name+" Colliders should be triggers! Please check the \"Is Trigger\" box in the inspector.");
		}
		void OnTriggerEnter(Collider cld) {
			var hero = cld.gameObject.GetComponent<Hero> ();
			if (hero == null)
				return;
			var prevID = CurrentGameSaveData.Data.LastCheckpoint;
			Debug.Log (
				"The last Checkpoint is now \""+gameObject.name+"\" (ID:"+ID+" )."
				+" The previous one was \""+All[prevID].gameObject.name+"\" (ID: "+prevID+")."
			);
			CurrentGameSaveData.Data.LastCheckpoint = ID;
		}
	}
}
