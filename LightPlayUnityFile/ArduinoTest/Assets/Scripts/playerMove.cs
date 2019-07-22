using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour {
	
	public string horizontalInputName = "Horizontal";
	public string veritcalInputName = "Vertical";
	public float movementSpeed = 6;
	private CharacterController charControl;
	 
	void Awake() {
		charControl = GetComponent<CharacterController>();
	}
	// Use this for initialization
	void Start () {
		
	}
	void playerMover() {
		float vertInput = Input.GetAxis(veritcalInputName) * movementSpeed;
		float horzInput = Input.GetAxis(horizontalInputName) * movementSpeed;

		Vector3 fwdMovement = transform.forward * vertInput;
		Vector3 rightMovement = transform.right * horzInput;

		charControl.SimpleMove(fwdMovement + rightMovement);

	}
	// Update is called once per frame
	void Update () {
		playerMover();
	}
}
