using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Se {
	
	public enum GroundType {
		Rock, Grass, Sand, Snow, Glass,
	}

	[RequireComponent(typeof(Collider))]
    public class Ground : MonoBehaviour {
		public GroundType Type;

		void Start() {
			Assert.IsTrue (gameObject.isStatic, "\"" + gameObject.name + "\": "+GetType().Name+"s should be static! Please check the \"Static\" box for this object in the inspector.");
			Assert.IsFalse (GetComponent<Collider>().isTrigger, "\"" + gameObject.name + "\": "+GetType().Name+" Colliders shouldn't be triggers! Please uncheck the \"Is Trigger\" box in the inspector.");
		}
		void OnCollisionEnter(Collision cls) {
			var hero = cls.gameObject.GetComponent<Hero> ();
			if (hero == null)
				return;
			Debug.Log("Hero is now touching the ground \""+gameObject.name+"\" (type: "+Type+").");
		}
		void OnCollisionLeave(Collision cls) {
			var hero = cls.gameObject.GetComponent<Hero> ();
			if (hero == null)
				return;
			Debug.Log("Hero is now leaving the ground \""+gameObject.name+"\" (type: "+Type+").");
		}
    }
}
