using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPunch : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		Invoke ("DestroyTheElement", 1);
	}

	void DestroyTheElement(){
		Destroy(GetComponent<Rigidbody> ());
		Destroy(GetComponent<CapsuleCollider> ());
		Destroy(gameObject);
	}
}
