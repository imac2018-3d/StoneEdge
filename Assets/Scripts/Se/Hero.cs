using UnityEngine;
using System.Collections;
using Utils;
using UnityEngine.Assertions;

namespace Se {

	[RequireComponent(typeof(CharacterController))]
	public class Hero : MonoBehaviour {

		// public attributes, tweakable in the inspector
		public FollowingCamera Camera;
		public float GroundMovementSpeedFactor = 15f;
		public float AirMovementSpeedFactor = 0.5f;
		public float JumpStrength = 80.0f;
		public float FallSpeedFactor = 3f;
		public int MaxLife = 5;

		// private or hidden attributes
		Fsm fsm = new Fsm(new HeroStates.Idle());
		internal int life;
		internal Vector3 moveDirection = Vector3.zero;

		void Start () {
			Assert.IsTrue(MaxLife > 0);
			Assert.IsNotNull (Camera);
			life = MaxLife;
			fsm.OnStart (gameObject);
		}
		void Update() {
			fsm.OnUpdate (gameObject);
		}
		void FixedUpdate() {
			fsm.OnFixedUpdate (gameObject);
		}

		public void ReceiveDamage(int amount) {
			life -= amount;
			if (life <= 0) {
				life = 0;
				// NOTE TODO: If we're here, the player died. Do something.
			}
		}

		public Vector3 GetMovementInput() {
			var input = InputActions.MovementDirection;
			var right = Camera.transform.right;
			var forward = Camera.transform.forward;
			right.y = 0;
			forward.y = 0;
			right.Normalize ();
			forward.Normalize ();
			return right * input.x + forward * input.y;
		}
	}

	namespace HeroStates {
		public class Idle: FsmState {
			public override FsmState OnUpdate (GameObject go) {
				var hero = go.GetComponent <Hero>();
				var ctrl = go.GetComponent <CharacterController> ();
				if (ctrl.isGrounded) {
					hero.moveDirection = hero.GetMovementInput () * hero.GroundMovementSpeedFactor;
					if (InputActions.Jumps)
						hero.moveDirection.y = hero.JumpStrength;
				} else {
					var v = hero.GetMovementInput () * hero.AirMovementSpeedFactor;
					var candidate = hero.moveDirection + v;
					candidate.y = 0f;
					candidate = Vector3.ClampMagnitude (candidate, hero.GroundMovementSpeedFactor);
					hero.moveDirection.x = candidate.x;
					hero.moveDirection.z = candidate.z;
				}
				hero.moveDirection += Physics.gravity * hero.FallSpeedFactor * Time.deltaTime;
				ctrl.Move(hero.moveDirection * Time.deltaTime);
				var dir = hero.moveDirection;
				dir.y = 0f;
				go.transform.LookAt (go.transform.position + dir);
				return this;
			}
		}

		public class Walk: FsmState {
			private Vector3 newPos;
			private float timeForMagnetic;

			public override FsmState OnUpdate (GameObject go){
				/*
				if (go.GetComponent<Hero> ().grounded && Input.GetKeyDown (KeyCode.Space)) {
					return new Jump ();
				}else if (go.GetComponent<Hero> ().grounded && Input.GetKeyDown (KeyCode.X)) {
					return new Punch ();
				}else if (go.GetComponent<Hero> ().grounded && Input.GetKeyDown (KeyCode.C)) {
					return new MagnetImpact ();
				}else if (InputActions.Dodges) {
					return new Dodge ();
				}
				*/
				return this;
			}

			public override FsmState OnFixedUpdate (GameObject go) {
				/*
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
				*/
				return this;
			}
		}

		public class Jump: FsmState {
			private Vector3 jumpMovement;

			public override FsmState OnUpdate (GameObject go) {
				/*
				jumpMovement = new Vector3 (0.0f, go.GetComponent<Hero> ().jumpValue, 0.0f);
				go.GetComponent<Hero>().rb.AddForce(jumpMovement, ForceMode.Impulse);
				*/
				return new Idle ();
			}
		}

		public class MagnetImpact: FsmState {
			public override FsmState OnUpdate (GameObject go) {
				/*
				go.GetComponent<Hero> ().playerEnabledToMoveMagnetImpact = false;
				go.GetComponent<Hero>().magnetImpactGo.SetActive (true);
				*/
				return new Idle();
			}
		}

		public class Punch: FsmState {
			public override FsmState OnUpdate (GameObject go) {
				/*
				go.GetComponent<Hero> ().playerEnabledToMovePunch = false;
				go.GetComponent<Hero>().punchGo.SetActive (true);
				*/
				return new Idle();
			}
		}

		public class Dodge: FsmState {
			public override FsmState OnFixedUpdate (GameObject go) {
				/*
				float moveHorizontal = Input.GetAxis ("Horizontal");  // Get the horizontal keyboard arrows "forces"
				float moveVertical = Input.GetAxis ("Vertical");  // Get the vertical keyboard arrows "forces"

				Vector3 velocity = go.GetComponent<Hero> ().rb.velocity;
				//Vector3 movement = new Vector3 (moveHorizontal * go.GetComponent<Hero> ().speed * 20, velocity.y, moveVertical * go.GetComponent<Hero> ().speed * 20);  // Create a vector to move the character 
				Vector3 movement = new Vector3 (moveHorizontal * go.GetComponent<Hero> ().speed * 20, velocity.y, moveVertical * go.GetComponent<Hero> ().speed * 20);  // Create a vector to move the character 

				go.GetComponent<Hero>().rb.velocity = movement;  // Move the character
				*/
				return new Idle();
			}
		}
	}
}