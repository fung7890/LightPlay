using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class preGame : MonoBehaviour {

	public int randNum;
	public Material mat1, mat2;
	public string[] positions;
	System.Random rnd = new System.Random();
	public GameObject currentCube;
	// Use this for initialization

	void Start () {
		print("PREGAME STARTING");
		randNum = rnd.Next(1, 10);
		currentCube = GameObject.FindWithTag(randNum.ToString());
		currentCube.GetComponent<MeshRenderer>().material = mat2;
	}

	// void reset() {

	// 	// reset to all white blocks 
	// 	for (int i=1; i < 11; i++) {
	// 		currentCube = GameObject.FindWithTag(i.ToString());
	// 		currentCube.GetComponent<MeshRenderer>().material = mat1;
	// 	}

	// 	// set new random block to 
	// 	randNum = rnd.Next(1, 10);
	// 	print("RESETTING RANDNUM TO: ", randNum);
	// 	currentCube = GameObject.FindWithTag(randNum.ToString());
	// 	currentCube.GetComponent<MeshRenderer>().material = mat2;		
	// }
	
	// Update is called once per frame
	void Update () {
		RaycastHit TheHit;

		if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out TheHit)){
			// print(TheHit.transform.name.GetType());
			if (randNum.ToString() == TheHit.transform.name) {
				currentCube.GetComponent<MeshRenderer>().material = mat1;
				randNum = rnd.Next(1, 10);
				currentCube = GameObject.FindWithTag(randNum.ToString());
				currentCube.GetComponent<MeshRenderer>().material = mat2;
			}
		}
	}	
}
