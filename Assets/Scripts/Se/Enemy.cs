// NOTE: This is a placeholder for the Enemy component.

using UnityEngine;
using System.Collections;
using Utils;

namespace Se {
	public class Enemy : MonoBehaviour {
		Fsm fsm = new Fsm(new EnemyStates.Idle());
		void Start () {
			fsm.AssertNotNull ();
			fsm.OnStart (gameObject);
		}
		void Update() {
			fsm.OnUpdate (gameObject);
		}
	}
	namespace EnemyStates {
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