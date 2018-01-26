// NOTE: This is a placeholder for the hero component.

using UnityEngine;
using System.Collections;
using Utils;

namespace Se {
	public class Hero : MonoBehaviour {
		Fsm fsm = new Fsm(new HeroStates.Idle());

		public Rigidbody rb;  // containt the hero's informations
		public bool grounded;
		public float lowingSpeed = 3;

		public float fallMultiplier = 3.5f; // the multiplier to make the falling down part quicker than the jumping part
		public float lowJumpMultiplier = 3f; // The multiplier to make the jumping part lower than the fall down

		public bool playerEnabledToMoveMagnetImpact = true;
		public bool playerEnabledToMovePunch = true;
		public GameObject magnetImpactGo;
		public GameObject punchGo;

		public bool rightDodge;
		public bool dodging;

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
				}else if (Input.GetKeyDown (KeyCode.X)) {
					return new Punch ();
				}else if (Input.GetKeyDown (KeyCode.C)) {
					return new MagnetImpact ();
				}else if (Input.GetKeyDown (KeyCode.N)) {
					go.GetComponent<Hero> ().rightDodge = true;
					return new Dodge ();
				}else if (Input.GetKeyDown (KeyCode.B)) {
					go.GetComponent<Hero> ().rightDodge = false;
					return new Dodge ();
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

		public class MagnetImpact: FsmState {
			public override FsmState OnUpdate (GameObject go) {
				// NOTE :: ORIENTATION A BOUGER EN FONCTION DE OU REGARDE LE HERO
				go.GetComponent<Hero> ().playerEnabledToMoveMagnetImpact = false;
				go.GetComponent<Hero>().magnetImpactGo.SetActive (true);
				
				return new Idle();
			}
		}

		public class Punch: FsmState {
			public override FsmState OnUpdate (GameObject go) {
				// NOTE :: ORIENTATION A BOUGER EN FONCTION DE OU REGARDE LE HERO
				go.GetComponent<Hero> ().playerEnabledToMovePunch = false;
				go.GetComponent<Hero>().punchGo.SetActive (true);

				return new Idle();
			}
		}

		public class Dodge: FsmState {
			public override FsmState OnUpdate (GameObject go) {
				if(!go.GetComponent<Hero>().rightDodge){
					var dodgeVectorMovement = Vector3.left * 20f; // CHANGER EN FONCTION DE OU REGARDE LE HERO
					go.GetComponent<Hero>().rb.AddForce(dodgeVectorMovement, ForceMode.Impulse);
					go.GetComponent<Hero> ().dodging = true;
				}
				if(go.GetComponent<Hero>().rightDodge){
					var dodgeVectorMovement = Vector3.right * 20f; // CHANGER EN FONCTION DE OU REGARDE LE HERO
					go.GetComponent<Hero>().rb.AddForce(dodgeVectorMovement, ForceMode.Impulse);
					go.GetComponent<Hero> ().dodging = true;
				}

				return new Idle();
			}
		}
	}
}