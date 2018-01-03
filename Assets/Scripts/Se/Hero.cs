// NOTE: This is a placeholder for the hero component.

using UnityEngine;
using System.Collections;
using Utils;

namespace Se {
	public class Hero : MonoBehaviour {
		Fsm fsm = new Fsm(new HeroStates.Idle());
		void Start () {
			fsm.AssertNotNull ();
			fsm.OnStart (gameObject);
		}
		void Update() {
			fsm.OnUpdate (gameObject);
		}
	}
	namespace HeroStates {
		public class Idle: FsmState {
			public override FsmState OnUpdate (GameObject go) {
				throw new System.NotImplementedException ();
			}
		}
		public class Walk: FsmState {
			public override FsmState OnUpdate (GameObject go) {
				throw new System.NotImplementedException ();
			}
		}
	}
}