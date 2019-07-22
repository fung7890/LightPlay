using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class onOff : MonoBehaviour {
	public SerialPort serial = new SerialPort ("COM3", 9600);
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void onLed() {
		print("ON");
		if (serial.IsOpen == false) {
			serial.Open();
		}
		serial.Write("N");
	}

	
	public void offLed() {
		print("OFF");

		if (serial.IsOpen == false) {
			serial.Open();
		}
		serial.Write("a");
	}
}
