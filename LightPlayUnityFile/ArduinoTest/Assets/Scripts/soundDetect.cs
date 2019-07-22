using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using UnityEngine.UI;


public class soundDetect : MonoBehaviour {

	private bool detected;
	public bool onScreen, onLED;
	Collider tmp_other;
	public SerialPort serial = new SerialPort ("COM3", 9600);
	GameObject onScreenNotification, canvas, onScreenImage;
	RectTransform onScreenNotificationPos;
	float screenHeight, screenWidth;
	int topLEDCount = 32;
	int sideLEDCount = 20;
	int c1, c2, LEDToAdd;
	Color onScreenColor;

	// void Awake() {
	// // 	print("test");
	// // 	// testing alpha for notification
	// 	onScreenImage = GameObject.FindWithTag("notifColor");
	// // 	// change alpha
	// 	onScreenColor = onScreenImage.GetComponent<Image>().color;
	// 	print(onScreenColor);
	// 	onScreenColor.a = 0.1f;
	// 	print(onScreenColor);

	// }
	// Use this for initialization
	void Start () {
		serial.Open(); // open for arduino

		onScreenNotification = GameObject.FindWithTag("onScreenNotification");
		canvas = GameObject.FindWithTag("canvas");

		onScreenNotificationPos = onScreenNotification.GetComponent<RectTransform>();

		screenWidth = canvas.transform.position.x;
		screenHeight = canvas.transform.position.y;
		
		
		// onScreenNotificationPos.sizeDelta = new Vector2((1134.0f*0.67f/21.5f*3.0f), 25.0f); // adjusting on-screen notif. size 

		// onScreenNotificationPos.anchoredPosition = new Vector3(screenWidth - 20, 260, 0); 
		// onScreenNotificationPos.transform.eulerAngles = new Vector3(0 , 0, 90);
	}

	void OnTriggerEnter(Collider other) {
		tmp_other = other;
		if (other.tag == "sound1") {
			print("DETECTED SOUND1");

			if (onScreen) {
				InvokeRepeating("onScreenInput", 0, 0.1f);
			}

			if (onLED){
				InvokeRepeating("arduinoInput", 0, 0.05f);
			}
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.tag == "sound1") { 
			print("EXITING SOUND");
			CancelInvoke();
		}
	}

	// returns which position needed corresponding to angle of object of interest in Unity 
	int LedPosCalc() {
		Vector3 otherDir = tmp_other.transform.position - this.transform.position;
		Vector3 forward = this.transform.forward; 
		float angle = Vector3.SignedAngle(otherDir, forward, Vector3.up);

		float led_pos = 0;
		int output_led_pos;
		// print(angle);
		angle *= -1;

		// angle to led pos 
		if (angle >= 45 & angle <= 135) { 				   // right side of monitor			
			led_pos = -(19.0f/90.0f)*angle + (57.0f/2.0f); // function to map to pos of led for right side 

		} else if (angle > 135 | angle < -135) { 		   // bottom of monitor			
			if (angle < 0) { 							   // conversion to assist with function
				angle = angle*-1 - 135;
			} else {
				angle = angle*-1 + 225;
			}
			led_pos = (31.0f/90.0f)*angle + 72.0f; 		   // function to map to pos of led for bottom side 

		} else if (angle >= -135 & angle <= -45) { 		   // left side of monitor			
			angle *= -1; 								   // conversion to assist with function
			led_pos = (19.0f/90.0f)*angle + (85.0f/2.0f);  // function to map to pos of led for left side 

		} else if (angle > -45 & angle < 45) { 			   // top of monitor
			angle = angle*-1 + 45; 	
			led_pos = (31.0f/90.0f)*angle + 20.0f; 	 	   // function to map to pos of led for left side 
		}
		
		output_led_pos = (int)System.Math.Round(led_pos, 0); 	   // round to nearest int 
		// print(output_led_pos);
		return output_led_pos;
	}

	// calculate distance: map it to range of color gradient and amount of leds to light
	void calcAdditionalValues() {
		float percentageDist;
		float dist = Vector3.Distance(tmp_other.transform.position, this.transform.position); // distance to sound, starts at max distance 
		percentageDist = dist / tmp_other.transform.parent.GetComponent<AudioSource>().maxDistance; // percentage based off max distance 

		// color calc: green to red
		c1 = (int)(percentageDist * 255);
		c2 = 255 - c1;

		// amount of leds to light based off distance with max of 4 and min of 1 (on each side)
		if (percentageDist <= 0.25) {
			LEDToAdd = 4;	
		} else if (percentageDist > 0.25 & percentageDist <= 0.5) {
			LEDToAdd = 3;	
		} else if (percentageDist > 0.5 & percentageDist <= 0.75) {
			LEDToAdd = 2;	
		} else {
			LEDToAdd = 1;	
		}
	}



	// send led position to arduino for led representation
	void arduinoInput() {
		calcAdditionalValues(); // calculate c1 and c2 values for color and amount of LEDs to add on the sides (more lights)
		int dataIn = LedPosCalc();

		if (serial.IsOpen == false) {
			serial.Open();
		}
		serial.Write(dataIn.ToString()+" "+LEDToAdd.ToString()+" "+c1.ToString()+" "+c2.ToString()+"\n");
	}

	// send position to Unity for on screen representation 
	void onScreenInput() {
		calcAdditionalValues();    // calculate amount of LEDs to add on the sides (more lights)
		int dataIn = LedPosCalc(); // position

		float indicatorPos;
		float tmpScreenWidth = screenWidth*2;   // preprocess screenWidth for use
		float tmpScreenHeight = screenHeight*2; // preprocess screenHeight for use


		// resize, rotate, and move on screen indicator (resize is off for now)
		if (dataIn <= 19) { // right side of monitor 
			// resizeOnScreenInput("right", tmpScreenWidth, tmpScreenHeight);
			indicatorPos = dataIn * (tmpScreenHeight / sideLEDCount) + (tmpScreenHeight / sideLEDCount)/2.0f ; // calculate position of indicator
			onScreenNotificationPos.transform.eulerAngles = new Vector3(0 , 0, 90); 					       // rotate to 90 degrees
			onScreenNotificationPos.anchoredPosition = new Vector3(tmpScreenWidth - 20, indicatorPos, 0);      // move it
		} else if (dataIn >= 20 & dataIn <= 51) { // top of monitor
			// resizeOnScreenInput("top", tmpScreenWidth, tmpScreenHeight);
			dataIn = -1*dataIn + 51; 																	       // rescale to 0 and make it so 0th pos is on the left
			indicatorPos = dataIn * (tmpScreenWidth / topLEDCount) + (tmpScreenWidth / topLEDCount)/2.0f ; 
			onScreenNotificationPos.transform.eulerAngles = new Vector3(0 , 0, 0); 						       // rotate to original form
			onScreenNotificationPos.anchoredPosition = new Vector3(indicatorPos, tmpScreenHeight - 20, 0);     // move it
		} else if (dataIn >= 52 & dataIn <= 71) { // left side of monitor
			// resizeOnScreenInput("right", tmpScreenWidth, tmpScreenHeight);
			dataIn = -1*dataIn + 71; 							 										       // rescale to 0 and make it so 0th pos is on the bottom
			indicatorPos = dataIn * (tmpScreenHeight / sideLEDCount) + (tmpScreenHeight / sideLEDCount)/2.0f ; 
			onScreenNotificationPos.transform.eulerAngles = new Vector3(0 , 0, 90); 
			onScreenNotificationPos.anchoredPosition = new Vector3( 20, indicatorPos, 0); 
		} else if (dataIn >= 72 & dataIn <= 103) { // bottom of monitor
			// resizeOnScreenInput("top", tmpScreenWidth, tmpScreenHeight);
			dataIn = dataIn - 72; 																		       // rescale to 0 
			indicatorPos = dataIn * (tmpScreenWidth / topLEDCount) + (tmpScreenWidth / topLEDCount)/2.0f ; 
			onScreenNotificationPos.transform.eulerAngles = new Vector3(0 , 0, 0); 						       // rotate to original form
			onScreenNotificationPos.anchoredPosition = new Vector3(indicatorPos, 20, 0); 				       // move it
		}
	}

	// to help with resizing of onscreen input
	void resizeOnScreenInput(string side, float tmpScreenWidth, float tmpScreenHeight) {	
		float notifSize;

		if (LEDToAdd == 4) { // closest, largest size
			notifSize = 21;
		} else if (LEDToAdd == 3) {
			notifSize = 13;
		} else if (LEDToAdd == 2) {
			notifSize = 7;
		} else {            // furthest, smallest size
			notifSize = 3;
		}

		if (side == "right") {      // for left and right side
			onScreenNotificationPos.sizeDelta = new Vector2((tmpScreenHeight / sideLEDCount * notifSize), 25.0f); // adjusting on-screen notif. size 
		} else if (side == "top") { // for top and bottom side
			onScreenNotificationPos.sizeDelta = new Vector2((tmpScreenWidth / topLEDCount * notifSize), 25.0f);   // adjusting on-screen notif. size 
		}
	}
}
