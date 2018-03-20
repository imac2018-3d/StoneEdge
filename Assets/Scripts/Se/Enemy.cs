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

        public Transform Target;
        public float maxDistanceSight = 5.0f;

        public float timeBetweenAttacks = 2.0f;
        private float lastAttackTimeElapsed = 0.0f;
        public bool timeToAttack () { return lastAttackTimeElapsed - Time.time > timeBetweenAttacks; }
        public void resetAttackTimer() { lastAttackTimeElapsed = Time.time; }

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
                var enemy = go.GetComponent<Enemy>();
                // TODO check if target is in sight
                if (! Physics.Raycast(go.transform.position, enemy.Target.position - go.transform.position)) // hero is visible
                {
                    if ((enemy.Target.position - go.transform.position).magnitude < enemy.maxDistanceSight) // hero is close enough
                    {
                        if(enemy.timeToAttack()) // it's time to attack
                        {
                            return new Attack();
                        } else // it's not time to attack, wait more
                        {
                            return this;
                        }
                        
                    }
                }
                return this;
			}

            // is this needed ?
            public override void OnEnterState(GameObject go)
            {
                // TODO
            }
            public override void OnLeaveState(GameObject go)
            {
                // TODO
            }
        }
		public class Walk: FsmState {
			public override FsmState OnUpdate (GameObject go) {
				return this;
			}
		}
        public class Attack: FsmState {
            public override FsmState OnUpdate(GameObject go)
            {
                return this;
            }
            public override void OnEnterState(GameObject go)
            {
                var enemy = go.GetComponent<Enemy>();
                enemy.resetAttackTimer();
            }
            public override void OnLeaveState(GameObject go)
            {
                // TODO
            }
        }
	}
}