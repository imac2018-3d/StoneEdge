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
		public float DodgeStrength = 25f;
		public float DodgeDuration = 1f;
		public float PunchColliderDuration = 0.4f;
		public float PunchCooldownDuration = 1f;
		public int MaxLife = 5;

		// private or hidden attributes
		Fsm fsm = new Fsm(new HeroStates.Movable());
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
			
		public void KeepAirKicking() {
			KeepPunching ();
		}
		public void KeepPunching() {
			if (canPunch) {
				doPunch ();
			}
		}

		float lastPunchStartTime = 0f;

		bool canPunch { get { return lastPunchStartTime==0f ? true : Time.time - lastPunchStartTime > PunchCooldownDuration; } }

		public void doPunch() {
			AudioManager.GetInstance ().PlayAction (AudioManager.Action.BasicAttack);
			lastPunchStartTime = Time.time;
			var go = GameObject.CreatePrimitive (PrimitiveType.Sphere);
			go.transform.SetParent (transform, false);
			go.transform.Translate (Vector3.forward, Space.Self);
			go.transform.Rotate(Vector3.right, 90f, Space.Self);
			go.AddComponent<ElectrickPunching> ();
			Destroy (go, PunchColliderDuration);
		}
	}

	namespace HeroStates {
		public class Movable: FsmState {
			public override FsmState OnUpdate (GameObject go) {
				var hero = go.GetComponent <Hero>();
				var ctrl = go.GetComponent <CharacterController> ();

				if (ctrl.isGrounded) {
					if (InputActions.Dodges) {
						AudioManager.GetInstance ().PlayAction (AudioManager.Action.Dodge);
						return new Dodge ();
					}
					if (InputActions.IsPunching) {
						hero.KeepPunching ();
					}
					hero.moveDirection = hero.GetMovementInput () * hero.GroundMovementSpeedFactor;
					if (InputActions.Jumps) {
						// AudioManager.GetInstance ().PlayAction (AudioManager.Action.Jump);
						hero.moveDirection.y = hero.JumpStrength;
					}
				} else {
					if (InputActions.IsAirKicking) {
						AudioManager.GetInstance ().PlayAction (AudioManager.Action.BasicAttack);
						hero.KeepAirKicking ();
					}
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

		public class Dodge: FsmState {
			float startTime;
			public override void OnEnterState (GameObject go) {
				var hero = go.GetComponent <Hero>();
				hero.moveDirection = go.transform.forward * hero.DodgeStrength;
				startTime = Time.time;
			}
			public override FsmState OnUpdate (GameObject go) {
				var hero = go.GetComponent <Hero>();
				var ctrl = go.GetComponent <CharacterController> ();
				hero.moveDirection = Vector3.Lerp (hero.moveDirection, Vector3.zero, (Time.time - startTime) / hero.DodgeDuration);
				ctrl.Move(hero.moveDirection * Time.deltaTime);
				if (hero.moveDirection.magnitude <= 0.5f)
					return new Movable ();
				return this;
			}
		}
	}
}