using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour {

	public float MoveX = 0;
	public float MoveZ = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		MoveX = Input.GetAxis("Horizontal");
		MoveZ = Input.GetAxis("Vertical");
	}
}
