// NOTE: This is a placeholder for the hero component.

using UnityEngine;
using System.Collections;
using Utils;

namespace Se {
	public class Hero : MonoBehaviour {
		Fsm fsm = new Fsm(new HeroStates.Idle());

		public Rigidbody rb;  // containt the hero's informations
		public bool playerEnabledToMove = true;
		public bool grounded;
		public float lowingSpeed = 3;

		public float fallMultiplier = 3.5f; // the multiplier to make the falling down part quicker than the jumping part
		public float lowJumpMultiplier = 3f; // The multiplier to make the jumping part lower than the fall down

		void Start () {
			fsm.AssertNotNull ();
			fsm.OnStart (gameObject);
			rb = GetComponent<Rigidbody> ();
		}

		// Detect collision with floor
		void OnCollisionEnter(Collision hit){
			if (hit.gameObject.name == "Terrain") grounded = true;
		}

		void OnCollisionExit(Collision hit){
			if (hit.gameObject.name == "Terrain") grounded = false;
		}

		void Update() {
				// To do a better jump
			if(rb.velocity.y < 0) {
				rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
			} else if (GetComponent<Hero>().rb.velocity.y > 0 && !Input.GetKeyDown (KeyCode.Space)) {
				rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
			}

			fsm.OnUpdate (gameObject);
		}
		void FixedUpdate() {
			fsm.OnFixedUpdate (gameObject);
		}
	}
	namespace HeroStates {
		public class Idle: FsmState {
			public override FsmState OnUpdate (GameObject go) {
				if (go.GetComponent<Hero> ().playerEnabledToMove)
					return new Walk ();
				return this;
			}
		}

		public class Walk: FsmState {
			private Vector3 newPos;

			public override FsmState OnUpdate (GameObject go){
				if (go.GetComponent<Hero> ().grounded && Input.GetKeyDown (KeyCode.Space)) {
					return new Jump ();
				}
				return this;
			}

			public override FsmState OnFixedUpdate (GameObject go) {
				go.GetComponent<Hero>().rb.constraints = RigidbodyConstraints.FreezeRotation;  // Used to avoid the character to fall down in an uphill

				float moveHorizontal = Input.GetAxis ("Horizontal");  // Get the horizontal keyboard arrows "forces"
				float moveVertical = Input.GetAxis ("Vertical");  // Get the vertical keyboard arrows "forces"

				Vector3 movement = new Vector3 (moveHorizontal / go.GetComponent<Hero> ().lowingSpeed, 0.0f, moveVertical / go.GetComponent<Hero> ().lowingSpeed);  // Create a vector to move the character 

				go.GetComponent<Hero>().rb.MovePosition (go.transform.position + movement);  // Move the character
				return this;
			}
		}

		public class Jump: FsmState {
			private Vector3 jumpMovement;
			public float jumpValue = 15.0f;

			public override FsmState OnUpdate (GameObject go) {
				jumpMovement = new Vector3 (0.0f, jumpValue, 0.0f);
				go.GetComponent<Hero>().rb.AddForce(jumpMovement, ForceMode.Impulse);
				return new Idle ();
			}
		}
	}
}