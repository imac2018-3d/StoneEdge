using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

	private Rigidbody rb;
	private GameObject target;

	public float fallMultiplier = 3.5f; // the multiplier to make the falling down part quicker than the jumping part
	public float lowJumpMultiplier = 3f; // The multiplier to make the jumping part lower than the fall down

	void Start () {
		rb = GetComponent<Rigidbody> ();
		target = GameObject.Find ("Ennemy");
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.P)) {

			var heading = target.transform.position - rb.transform.position;
			float dot = Vector3.Dot (heading, rb.transform.forward);

			if (dot > 1)
				Debug.Log ("TOUCHED");
			else
				Debug.Log ("MISSED");
		} else if (Input.GetKeyDown (KeyCode.N)) {
			Debug.Log ("NOTHING");
		}
	}
}
