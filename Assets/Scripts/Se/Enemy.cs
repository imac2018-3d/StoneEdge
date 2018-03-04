// NOTE: This is a placeholder for the Enemy component.

using UnityEngine;
using System.Collections;
using Utils;

namespace Se {
	public class Enemy : MonoBehaviour {
		Fsm fsm = new Fsm(new EnemyStates.Idle());

		public Rigidbody rb;

		public float fallMultiplier = 3.5f; // the multiplier to make the falling down part quicker than the jumping part
		public float lowJumpMultiplier = 3f; // The multiplier to make the jumping part lower than the fall down

		public static int lifeMax = 3;
		public int life = lifeMax;

		void Start () {
			rb = GetComponent<Rigidbody> ();
			fsm.AssertNotNull ();
			fsm.OnStart (gameObject);
		}
		void Update() {
			if(rb.velocity.y < 0) {
				rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier) * Time.deltaTime;
			} else if (rb.velocity.y > 0 && !Input.GetKeyDown (KeyCode.Space)) {
				rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier) * (Time.deltaTime);
			}

			fsm.OnUpdate (gameObject);
		}

		void looseLife() {
			life--;
		}
	}
	namespace EnemyStates {
		public class Idle: FsmState {
			public override FsmState OnUpdate (GameObject go) {
				return this;
			}
		}
		public class Walk: FsmState {
			public override FsmState OnUpdate (GameObject go) {
				return this;
			}
		}
	}
}