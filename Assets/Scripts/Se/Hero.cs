// NOTE: This is a placeholder for the hero component.

using UnityEngine;
using System.Collections;
using Utils;

namespace Se {
	public class Hero : MonoBehaviour {
		Fsm fsm = new Fsm(new HeroStates.Idle());

		public Rigidbody rb;  // containt the hero's informations
		public bool grounded;
		public float speed = 15f;
		public float speedRotation = 30f;

		public float jumpValue = 80.0f;
		public float fallMultiplier = 4.5f; // the multiplier to make the falling down part quicker than the jumping part
		public float lowJumpMultiplier = 3f; // The multiplier to make the jumping part lower than the fall down

		public bool playerEnabledToMoveMagnetImpact = true;
		public bool playerEnabledToMovePunch = true;
		public GameObject magnetImpactGo;
		public GameObject punchGo;

		public bool rightDodge;
		public bool dodging;

		private int maxLife = 5;
		public int life = 5;

		void Start () {
			fsm.AssertNotNull ();
			fsm.OnStart (gameObject);
			rb = GetComponent<Rigidbody> ();
			magnetImpactGo.SetActive (false);
			punchGo.SetActive (false);
			dodging = false;
		}

		// Detect collision with floor
		void OnCollisionEnter(Collision hit){
			if (hit.gameObject.name == "Terrain") grounded = true;
		}

		void OnCollisionExit(Collision hit){
			if (hit.gameObject.name == "Terrain") grounded = false;
		}

		void Update() {
			rb.constraints = RigidbodyConstraints.FreezeRotation;  // Used to avoid the character to fall down in an uphill

				// To do a better jump
			if(rb.velocity.y < 0) {
				rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
			} else if (	rb.velocity.y > 0 && !Input.GetKeyDown (KeyCode.Space)) {
				rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
			}

			if(!playerEnabledToMoveMagnetImpact)
				Invoke ("EnablingToMoveMagnetImpact", 0.5f);
			if(!playerEnabledToMovePunch)
				Invoke ("EnablingToMovePunch", 0.3f);

			fsm.OnUpdate (gameObject);
		}
		void FixedUpdate() {
			fsm.OnFixedUpdate (gameObject);
		}

		void EnablingToMoveMagnetImpact() {
			magnetImpactGo.SetActive (false);
			playerEnabledToMoveMagnetImpact = true;
		}
		void EnablingToMovePunch() {
			punchGo.SetActive (false);
			playerEnabledToMovePunch = true;
		}

		void FullLife() {
			life = maxLife;
		}

		void LooseLife() {
			life--;
		}

	}
	namespace HeroStates {
		public class Idle: FsmState {

			private float timeForMagnetic;

			public override FsmState OnUpdate (GameObject go) {
				if (go.GetComponent<Hero> ().playerEnabledToMoveMagnetImpact && go.GetComponent<Hero> ().playerEnabledToMovePunch)
					return new Walk ();
				else if (Input.GetKeyDown (KeyCode.X)) {
					return new Punch ();
				}
				return this;
			}
		}

		public class Walk: FsmState {
			private Vector3 newPos;
			private float timeForMagnetic;

			public override FsmState OnUpdate (GameObject go){
				if (go.GetComponent<Hero> ().grounded && Input.GetKeyDown (KeyCode.Space)) {
					return new Jump ();
				}else if (go.GetComponent<Hero> ().grounded && Input.GetKeyDown (KeyCode.X)) {
					return new Punch ();
				}else if (go.GetComponent<Hero> ().grounded && Input.GetKeyDown (KeyCode.C)) {
					return new MagnetImpact ();
				}else if (InputActions.Dodges) {
					return new Dodge ();
				}
				return this;
			}

			public override FsmState OnFixedUpdate (GameObject go) {
				float moveHorizontal = Input.GetAxis ("Horizontal");  // Get the horizontal keyboard arrows "forces"
				float moveVertical = Input.GetAxis ("Vertical");  // Get the vertical keyboard arrows "forces"

				Vector3 velocity = go.GetComponent<Hero> ().rb.velocity;
				Vector3 movement = new Vector3 (moveHorizontal * go.GetComponent<Hero> ().speed, velocity.y, moveVertical * go.GetComponent<Hero> ().speed);  // Create a vector to move the character 

				go.GetComponent<Hero>().rb.velocity = movement;  // Move the character

				Vector3 forward = go.GetComponent<Hero> ().rb.transform.forward;
				forward = forward.normalized;
				forward.y = 0f;

				Vector3 directionTo = movement.normalized ;
				directionTo = directionTo.normalized;
				directionTo.y = 0f;

				Vector3 between = Vector3.Lerp (forward, directionTo, 0.2f);
				//Debug.Log ("Between : "+ between.x + ", " + between.y + ", " + between.z);
				Quaternion reorientation = Quaternion.LookRotation (between, forward);
				//reorientation.Set(reorientation.x, reorientation.y, 0f, reorientation.z);

				//go.GetComponent<Hero> ().rb.rotation = reorientation;

				return this;
			}
		}

		public class Jump: FsmState {
			private Vector3 jumpMovement;

			public override FsmState OnUpdate (GameObject go) {
				jumpMovement = new Vector3 (0.0f, go.GetComponent<Hero> ().jumpValue, 0.0f);
				go.GetComponent<Hero>().rb.AddForce(jumpMovement, ForceMode.Impulse);
				return new Idle ();
			}
		}

		public class MagnetImpact: FsmState {
			public override FsmState OnUpdate (GameObject go) {
				go.GetComponent<Hero> ().playerEnabledToMoveMagnetImpact = false;
				go.GetComponent<Hero>().magnetImpactGo.SetActive (true);
				
				return new Idle();
			}
		}

		public class Punch: FsmState {
			public override FsmState OnUpdate (GameObject go) {
				go.GetComponent<Hero> ().playerEnabledToMovePunch = false;
				go.GetComponent<Hero>().punchGo.SetActive (true);

				return new Idle();
			}
		}

		public class Dodge: FsmState {
			public override FsmState OnFixedUpdate (GameObject go) {
				float moveHorizontal = Input.GetAxis ("Horizontal");  // Get the horizontal keyboard arrows "forces"
				float moveVertical = Input.GetAxis ("Vertical");  // Get the vertical keyboard arrows "forces"

				Vector3 velocity = go.GetComponent<Hero> ().rb.velocity;
				//Vector3 movement = new Vector3 (moveHorizontal * go.GetComponent<Hero> ().speed * 20, velocity.y, moveVertical * go.GetComponent<Hero> ().speed * 20);  // Create a vector to move the character 
				Vector3 movement = new Vector3 (moveHorizontal * go.GetComponent<Hero> ().speed * 20, velocity.y, moveVertical * go.GetComponent<Hero> ().speed * 20);  // Create a vector to move the character 

				go.GetComponent<Hero>().rb.velocity = movement;  // Move the character

				return new Idle();
			}
		}
	}
}