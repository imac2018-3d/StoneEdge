using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectrickPunching : MonoBehaviour {

	private Rigidbody heroRigidbody;
	private Rigidbody ennemyRigidBody;


	// Use this for initialization
	void Start () {
	}

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
			Vector3 ejectEnnemy = heroToEnnemyVectorNormalized * 3.0f + Vector3.up * 3.0f;

			ennemyRigidBody.AddForce (ejectEnnemy, ForceMode.Impulse);
			Debug.Log ("KICKKKKKK");
		}
	}
	
	// Update is called once per frame
	void Update () {
	}
}
