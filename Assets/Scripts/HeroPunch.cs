using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroPunch : MonoBehaviour {

	private Rigidbody rb;
	public GameObject punch;
	private GameObject punchClone;
	public int punchShift = 3;

	public float fallMultiplier = 3.5f; // the multiplier to make the falling down part quicker than the jumping part
	public float lowJumpMultiplier = 3f; // The multiplier to make the jumping part lower than the fall down


	void Start () {
		rb = GetComponent<Rigidbody> ();
	}

	void FixedUpdate () {
		if (Input.GetKeyDown (KeyCode.X)) {
			Vector3 rbPos = rb.position;
			Vector3 shift = Vector3.forward * punchShift + Vector3.forward * punchShift;   // NOTE :: A CHANGER EN FONCTION DE OU REGARDE LE HERO
			Quaternion rbRotation = rb.rotation;
			Quaternion punchRotation = new Quaternion (0.0f, 90.0f, 90.0f, 0.0f);

			if(GameObject.Find("Hero").GetComponent<PlayerController>()){
				GameObject.Find("Hero").GetComponent<PlayerController>().playerEnabledToMove = false;
			}else{
				Debug.Log("No Motor Attached! (Hero in HeroPunch)");
			}

			punchClone = Instantiate (punch, rbPos + shift, punchRotation);
			Rigidbody PunchRigidbody = punchClone.AddComponent<Rigidbody>(); // Add the rigidbody.
			PunchRigidbody.mass = 1; 

			Invoke ("EnablingToMove", 0.3f);
		}

		/*if (Input.GetKeyUp (KeyCode.X)) {
			if(GameObject.Find("Hero").GetComponent<PlayerController>()){
				GameObject.Find("Hero").GetComponent<PlayerController>().playerEnabledToMove = true;
			}else{
				Debug.Log("No Motor Attached!");
			}
		}*/
	}

	void EnablingToMove() {
		if(GameObject.Find("Hero").GetComponent<PlayerController>()){
			GameObject.Find("Hero").GetComponent<PlayerController>().playerEnabledToMove = true;
		}else{
			Debug.Log("No Motor Attached! (Hero in HeroPunch)");
		}
	}
}
