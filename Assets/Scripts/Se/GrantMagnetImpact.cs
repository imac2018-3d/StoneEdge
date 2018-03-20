using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Se {
	[RequireComponent(typeof(Collider))]
	public class GrantMagnetImpact: MonoBehaviour {
		void Start() {
			Assert.IsTrue (gameObject.isStatic, "\"" + gameObject.name + "\": "+GetType().Name+"s should be static! Please check the \"Static\" box for this object in the inspector.");
			Assert.IsTrue (GetComponent<Collider>().isTrigger, "\"" + gameObject.name + "\": "+GetType().Name+" Colliders should be triggers! Please check the \"Is Trigger\" box in the inspector.");
		}
		void OnTriggerEnter(Collider cld) {
			var hero = cld.gameObject.GetComponent<Hero> ();
			if (hero == null)
				return;
			Debug.Log ("Granting MagnetImpact power to the Hero!" + (CurrentGameSaveData.Data.HasAcquiredMagnetImpact ? " (was already acquired)" : ""));
			StartCoroutine (ShowExplanations());
			CurrentGameSaveData.Data.HasAcquiredMagnetImpact = true;
		}

		IEnumerator ShowExplanations(){
	        PlayerExplanationsManager.GetInstance ().ShowMagnetImpact ();
	        yield return new WaitForSeconds(10);
	        PlayerExplanationsManager.GetInstance ().HideMagnetImpact ();
	    }
	}
}
