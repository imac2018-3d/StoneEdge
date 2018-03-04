using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchPhysic : MonoBehaviour {

	public float forcePunch = 17f;

	private Rigidbody heroRigidbody;
	private Rigidbody ennemyRigidBody;

	public float fallMultiplier = 3.5f; // The multiplier to make the falling down part quicker than the jumping part
	public float lowJumpMultiplier = 3f; // The multiplier to make the jumping part lower than the fall down
	public float acceleration = 10f; // The multiplier to accelerate the action


	void OnTriggerEnter(Collider hit)
	{
		if (hit.gameObject.GetComponent<EnnemyType>() != null) {

			ennemyRigidBody = hit.gameObject.GetComponent<Rigidbody>();
			// Fin the GameObject of the Hero
			if(GameObject.Find("Hero")){
				heroRigidbody = GameObject.Find ("Hero").GetComponent<Rigidbody> ();
			}else{
				Debug.Log("No Hero found ! (Error from : ElectrickPunching)");
			}

				// Create the vector for the ejection by calculating the vector between the hero and the ennemy
			Vector3 heroToEnnemyVectorNormalized = (ennemyRigidBody.position - heroRigidbody.position).normalized;
			// Vector up puis normalize
			//Vector3 ejectEnnemy = heroToEnnemyVectorNormalized * forcePunch + Vector3.up * forcePunch;
			Vector3 ejectEnnemy = heroToEnnemyVectorNormalized * forcePunch + Vector3.up * forcePunch;
			//ennemyRigidBody.AddForce (ejectEnnemy, ForceMode.Impulse);
			ennemyRigidBody.velocity = ejectEnnemy;

			// looseLife de l'ennemi
		}
	}
}
