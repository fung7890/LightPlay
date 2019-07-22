using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerLook : MonoBehaviour {

	public string mouseXInputName = "Mouse X", mouseYInputName = "Mouse Y";
	public float mouseSensitivity = 150.0f;
	private float xAxisClamp;
	public Transform playerBody;

	void Awake() {
		lockCursor();
		xAxisClamp = 0;
	}


	void lockCursor() {
		Cursor.lockState = CursorLockMode.Locked;
	}

	void camRotate() {
		float mouseX = (Input.GetAxis(mouseXInputName))*mouseSensitivity*Time.deltaTime;
		float mouseY = (Input.GetAxis(mouseYInputName))*mouseSensitivity*Time.deltaTime;

		xAxisClamp += mouseY;

		if (xAxisClamp > 90.0f) {
			xAxisClamp = 90.0f;
			mouseY = 0.0f;	
		} else if (xAxisClamp < -90.0f) {
			xAxisClamp = -90.0f;
			mouseY = 0.0f;				
		}
		
		transform.Rotate(Vector3.left * mouseY);
		playerBody.Rotate(Vector3.up * mouseX);

	}
	
	// Update is called once per frame
	void Update () {
		camRotate(); 
	}
}
