using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using Utils;

namespace Se
{

	[RequireComponent(typeof(CharacterController))]
	public class Enemy : MonoBehaviour
	{
		// public attributes, tweakable in the inspector
		public float GroundMovementSpeedFactor = 15f;
		public int MaxLife = 5;
		public float MaxDistFromInitialPosition = 3;
		public float IdleIntervalDuration = 5;
		public float MaxDistToHeroToAttack = 20;
		public float MaxDistFromInitialPosWhileAttacking = 40;

		// private or hidden attributes
		Fsm fsm = new Fsm(new EnemyStates.Movable());
		internal int life;
		internal Vector3 moveDirection = Vector3.zero;
		internal Animator animator;
		internal Vector3 initialPosition;
		internal Vector3 currentDestination;
		internal float lastIdleDestinationChange;
		internal Hero hero;

		internal Vector3 randomDestinationInIdleCircle()
		{
			Vector2 randomPoint = UnityEngine.Random.insideUnitCircle;
			return initialPosition + MaxDistFromInitialPosition * new Vector3(randomPoint.x, 0, randomPoint.y);
		}

		void Start()
		{
			Assert.IsTrue(MaxLife > 0);
			life = MaxLife;
			initialPosition = transform.position;
			lastIdleDestinationChange = Time.time;
			hero = FindObjectOfType<Hero>();
			Assert.IsNotNull(hero, "Pas de héros dans le scene!!!!!!!!!!!!!!!!");
			animator = GetComponentInChildren<Animator>();
			fsm.OnStart(gameObject);
		}
		void Update()
		{
			fsm.OnUpdate(gameObject);
		}
		void FixedUpdate()
		{
			fsm.OnFixedUpdate(gameObject);
		}

		public void ReceiveDamage(int amount)
		{
			life -= amount;
			if (life <= 0)
			{
				life = 0;
				// NOTE TODO: If we're here, the enemy died.
			}
		}

		internal bool seeHeroCloseEnough()
		{
			// if close enough
			if((hero.transform.position - transform.position).magnitude > MaxDistToHeroToAttack)
			{
				return false;
			}
			// if hero visible
			RaycastHit hit;
			if (Physics.Raycast(transform.position, hero.transform.position - transform.position, out hit))
			{
				if(hit.collider.gameObject.GetComponent<Hero>() != null)
				{
					return false;
				}
			}
			return true;
		}

		internal bool arrivedToDestination()
		{
			return (currentDestination - transform.position).sqrMagnitude < 0.01;
		}

		public Vector3 GetMovementInput()
		{
			if (!arrivedToDestination())
			{
				return (currentDestination - transform.position).normalized;
			}
			else
			{
				return Vector3.zero;
			}
		}
	}

	namespace EnemyStates
	{
		public class Movable : FsmState
		{
			public override FsmState OnUpdate(GameObject go)
			{
				var enemy = go.GetComponent<Enemy>();
				var ctrl = go.GetComponent<CharacterController>();

				// update idle walk destination
				if(Time.time - enemy.lastIdleDestinationChange > enemy.IdleIntervalDuration
					|| enemy.arrivedToDestination())
				{
					enemy.currentDestination = enemy.randomDestinationInIdleCircle();
					enemy.lastIdleDestinationChange = Time.time;
				}

				// test if enemy has to attack hero
				if (enemy.seeHeroCloseEnough())
				{
					return new Attacking();
				}

				if (ctrl.isGrounded)
				{
					enemy.moveDirection = enemy.GetMovementInput() * enemy.GroundMovementSpeedFactor;
				}
				enemy.moveDirection += Physics.gravity * Time.deltaTime;
				ctrl.Move(enemy.moveDirection * Time.deltaTime);

				var dir = enemy.moveDirection;
				dir.y = 0f;
				go.transform.LookAt(go.transform.position + dir);

				return this;
			}
		}
		public class Attacking : FsmState
		{
			public override FsmState OnUpdate(GameObject go)
			{
				var enemy = go.GetComponent<Enemy>();
				var ctrl = go.GetComponent<CharacterController>();

				// update destination
				enemy.currentDestination = enemy.hero.transform.position;

				// need to give up?
				if(!enemy.seeHeroCloseEnough() 
					|| (enemy.transform.position - enemy.initialPosition).magnitude > enemy.MaxDistFromInitialPosWhileAttacking)
				{
					return new Movable();
				}

				if (ctrl.isGrounded)
				{
					enemy.moveDirection = enemy.GetMovementInput() * enemy.GroundMovementSpeedFactor;
				}
				enemy.moveDirection += Physics.gravity * Time.deltaTime;
				ctrl.Move(enemy.moveDirection * Time.deltaTime);

				var dir = enemy.moveDirection;
				dir.y = 0f;
				go.transform.LookAt(go.transform.position + dir);

				return this;
			}
		}
	}
}