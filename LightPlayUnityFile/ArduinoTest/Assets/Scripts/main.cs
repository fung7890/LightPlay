using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using UnityEngine.UI;

public class main : MonoBehaviour {

	private GameObject cam, preGameObj, player, notifColor, contButton, instructionTxt;
	private AudioSource s1,s2,s3,s4,s5,s6,s7,s8,s9,s10,s11,s12,s13,s14,s15,s16;
	private Transform midOfPreGame;
	System.Random rnd = new System.Random();
	int randNum, nothing;
	float currTimerValue, noticeabilityTimeValue;
	bool secondaryTaskDone, recordNoticeability;
	List<List<float>> allTimes = new List<List<float>>(); // for all conditions 
	List<List<float>> allNoticeabilityTimes = new List<List<float>>(); // for all noticeabilityTimes
	List<float> conditionTimes = new List<float>(); // for each condition
	List<float> noticeabilityTimes = new List<float>(); // for noticeability times 
	Vector3 lookDir;

	// old static order 
	// int[] order1 = {2,5,6,16,10,3,7,11,14,12,9,4,12,9,5,3,6,13,14,8};
	// int[] order2 = {6,11,9,3,16,9,8,5,13,12,12,7,5,2,4,10,3,14,6,14};
	// int[] order3 = {13,11,8,2,5,9,9,7,6,6,12,4,14,3,3,14,5,16,12,10};

	// test order
	// int[] order1 = {16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16};
	// int[] order2 = {16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16};
	// int[] order3 = {16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16};

	// ordering
	int[] order1 = new int[20];
	int[] order2 = new int[20];
	int[] order3 = new int[20];

	int[] tmpOrder;
	List<int> conditionsRan = new List<int>(); // conditions already ran
	int currTarget;
	Dictionary<int, AudioSource> soundDict = new Dictionary<int, AudioSource>();
	Dictionary<int, SphereCollider> trigDict = new Dictionary<int, SphereCollider>();
	StringBuilder csv;
	private SphereCollider t1,t2,t3,t4,t5,t6,t7,t8,t9,t10,t11,t12,t13,t14,t15,t16;
	private bool pause;

	void Awake() {
		pause = false;
		secondaryTaskDone = true;
		recordNoticeability = false;
		cam = GameObject.FindWithTag("MainCamera");
		cam.GetComponent<preGame>().enabled = false;
		preGameObj = GameObject.FindWithTag("preGameObject");
		player = GameObject.FindWithTag("Player");
		instructionTxt = GameObject.FindWithTag("instruction");

		s1 = GameObject.Find("sound1").GetComponent<AudioSource>();
		s2 = GameObject.Find("sound2").GetComponent<AudioSource>();
		s3 = GameObject.Find("sound3").GetComponent<AudioSource>();
		s4 = GameObject.Find("sound4").GetComponent<AudioSource>();
		s5 = GameObject.Find("sound5").GetComponent<AudioSource>();
		s6 = GameObject.Find("sound6").GetComponent<AudioSource>();
		s7 = GameObject.Find("sound7").GetComponent<AudioSource>();
		s8 = GameObject.Find("sound8").GetComponent<AudioSource>();
		s9 = GameObject.Find("sound9").GetComponent<AudioSource>();
		s10 = GameObject.Find("sound10").GetComponent<AudioSource>();
		s11 = GameObject.Find("sound11").GetComponent<AudioSource>();
		s12 = GameObject.Find("sound12").GetComponent<AudioSource>();
		s13 = GameObject.Find("sound13").GetComponent<AudioSource>();
		s14 = GameObject.Find("sound14").GetComponent<AudioSource>();
		s15 = GameObject.Find("sound15").GetComponent<AudioSource>();
		s16 = GameObject.Find("sound16").GetComponent<AudioSource>();

		t1 = GameObject.FindWithTag("sound1").GetComponent<SphereCollider>();
		t2 = GameObject.FindWithTag("sound2").GetComponent<SphereCollider>();
		t3 = GameObject.FindWithTag("sound3").GetComponent<SphereCollider>();
		t4 = GameObject.FindWithTag("sound4").GetComponent<SphereCollider>();
		t5 = GameObject.FindWithTag("sound5").GetComponent<SphereCollider>();
		t6 = GameObject.FindWithTag("sound6").GetComponent<SphereCollider>();
		t7 = GameObject.FindWithTag("sound7").GetComponent<SphereCollider>();
		t8 = GameObject.FindWithTag("sound8").GetComponent<SphereCollider>();
		t9 = GameObject.FindWithTag("sound9").GetComponent<SphereCollider>();
		t10 = GameObject.FindWithTag("sound10").GetComponent<SphereCollider>();
		t11 = GameObject.FindWithTag("sound11").GetComponent<SphereCollider>();
		t12 = GameObject.FindWithTag("sound12").GetComponent<SphereCollider>();
		t13 = GameObject.FindWithTag("sound13").GetComponent<SphereCollider>();
		t14 = GameObject.FindWithTag("sound14").GetComponent<SphereCollider>();
		t15 = GameObject.FindWithTag("sound15").GetComponent<SphereCollider>();
		t16 = GameObject.FindWithTag("sound16").GetComponent<SphereCollider>();

		soundDict.Add(1,s1);
		soundDict.Add(2,s2);
		soundDict.Add(3,s3);
		soundDict.Add(4,s4);
		soundDict.Add(5,s5);
		soundDict.Add(6,s6);
		soundDict.Add(7,s7);
		soundDict.Add(8,s8);
		soundDict.Add(9,s9);
		soundDict.Add(10,s10);
		soundDict.Add(11,s11);
		soundDict.Add(12,s12);
		soundDict.Add(13,s13);
		soundDict.Add(14,s14);
		soundDict.Add(15,s15);
		soundDict.Add(16,s16);

		trigDict.Add(1,t1);
		trigDict.Add(2,t2);
		trigDict.Add(3,t3);
		trigDict.Add(4,t4);
		trigDict.Add(5,t5);
		trigDict.Add(6,t6);
		trigDict.Add(7,t7);
		trigDict.Add(8,t8);
		trigDict.Add(9,t9);
		trigDict.Add(10,t10);
		trigDict.Add(11,t11);
		trigDict.Add(12,t12);
		trigDict.Add(13,t13);
		trigDict.Add(14,t14);
		trigDict.Add(15,t15);
		trigDict.Add(16,t16);

		// setting up ordering
		for(int i = 0; i < 20; i++){
			order1[i] = rnd.Next(1,17);
			order2[i] = rnd.Next(1,17);
			order3[i] = rnd.Next(1,17);
		} 

		// print order for test;
		// for(int j=0; j<20; j++) {
		// 	print(order1[j].ToString() + " " + order2[j].ToString() + " " + order3[j].ToString());
		// }
	}
	
	// Use this for initialization
	void Start () {
		csv = new StringBuilder();  
		player.GetComponent<soundDetect>().resetLight();
		notifColor = GameObject.FindWithTag("notifColor");
		notifColor.SetActive(false);
		contButton = GameObject.FindWithTag("continue");
		contButton.SetActive(false);

		StartCoroutine(run());
		currTarget = 0;

		s1.enabled = false;
		s2.enabled = false;
		s3.enabled = false;
		s4.enabled = false;
		s5.enabled = false;
		s6.enabled = false;
		s7.enabled = false;
		s8.enabled = false;
		s9.enabled = false;
		s10.enabled = false;
		s11.enabled = false;
		s12.enabled = false;
		s13.enabled = false;
		s14.enabled = false;
		s15.enabled = false;
		s16.enabled = false;

		t1.enabled = false;
		t2.enabled = false;
		t3.enabled = false;
		t4.enabled = false;
		t5.enabled = false;
		t6.enabled = false;
		t7.enabled = false;
		t8.enabled = false;
		t9.enabled = false;
		t10.enabled = false;
		t11.enabled = false;
		t12.enabled = false;
		t13.enabled = false;
		t14.enabled = false;
		t15.enabled = false;
		t16.enabled = false;
	}
	
	// run game 
	void primaryTask() {

		// turn on object of pregame 
		preGameObj.SetActive(true);

		midOfPreGame = GameObject.FindWithTag("5").transform; // middle box of pregame
		Vector3 targetPos = new Vector3(midOfPreGame.transform.position.x, player.transform.position.y, midOfPreGame.transform.position.z);
		player.transform.LookAt(targetPos);

		cam.GetComponent<preGame>().enabled = true;
	}


	IEnumerator run()
    {	
		// on each condition
		for (int x=0; x<3; x++) {
			// random condition selection 
			int condition = rnd.Next(1, 4);

			// conditions: 1 - audio, 2 - onScreen, 3 - LED 
			while (conditionsRan.Contains(condition)){
				condition = rnd.Next(1,4);
			}
			conditionsRan.Add(condition);

			// pauses for TLX and instruction
			pause = true;
			contButton.SetActive(true);
			instructionTxt.SetActive(true);
			// display text for instruction
			if (condition == 1) {
				displayText(@"First Task: Hover your mouse over the highlighted box, it will continously change so try your best to follow it.

Second Task: (Please put on the headset) Click on the box you feel is making the sound. You won't progress until you find the right box so just keep trying!");		
			} else if (condition == 2) {
				displayText(@"First Task: Hover your mouse over the highlighted box, it will continously change so try your best to follow it.

Second Task: Click on the box when the on-screen indicator is in front you (top of the screen). You won't progress until you find the right box so just keep trying!");	
			} else {
				displayText(@"First Task: Hover your mouse over the highlighted box, it will continously change so try your best to follow it.
				
Second Task: Click on the box when the ambient light indicator is in front you (top of the monitor). You won't progress until you find the right box so just keep trying!");	
			}
			yield return new WaitUntil(() => pause == false);
			instructionTxt.SetActive(false);
			contButton.SetActive(false);

			currTarget = 0;

			if (x==0){
				tmpOrder = order1;
			} else if (x==1){
				tmpOrder = order2;
			} else {
				tmpOrder = order3;
			}

			for (int i=0; i<2; i++){ // each iteration 
				currTimerValue = 0; // reset acquistion time 
				noticeabilityTimeValue = 0; // reset noticeability time 

				print("VALUE IS");
				print(currTarget);
				
				primaryTask();
				randNum = rnd.Next(5, 10);
				yield return new WaitForSeconds(randNum);


				cam.GetComponent<preGame>().enabled = false; // turn off cam select for primary task

				// start secondary task
				secondaryTaskDone = false;

				// start recording noticeability time
				recordNoticeability = true;

				// wait until spacebar is pressed for noticeability
				// pause = true;
				// yield return new WaitUntil(() => pause == false);  

				// record noticeability time and turn off recording
				// recordNoticeability = false;
				// noticeabilityTimes.Add(noticeabilityTimeValue);

				// print("recorded noticeability, now recording acquisition time");

				// preGameObj.SetActive(false); // turn off object of pregame 	


				//  audio only
				if (condition == 1) { // CHANGE FROM TRIGDICT TO ORDER1DICT
					soundDict[tmpOrder[currTarget]].enabled = true; // turn on audiosource for current target
					yield return new WaitUntil(() => secondaryTaskDone == true);
					soundDict[tmpOrder[currTarget]].enabled = false; // turn off audiosource for current target

				// onscreen
				} else if (condition == 2) { 
					player.GetComponent<soundDetect>().onScreenActivate();
					trigDict[tmpOrder[currTarget]].enabled = true; // turn on audiosource for current target
					yield return new WaitUntil(() => secondaryTaskDone == true);
					trigDict[tmpOrder[currTarget]].enabled = false; // turn off audiosource for current target
					notifColor.SetActive(false);

				// LED
				} else {  
					player.GetComponent<soundDetect>().onLEDActivate();
					trigDict[tmpOrder[currTarget]].enabled = true; // turn on audiosource for current target
					yield return new WaitUntil(() => secondaryTaskDone == true);
					trigDict[tmpOrder[currTarget]].enabled = false; // turn off audiosource for current target
				}


				player.GetComponent<soundDetect>().resetLight();

				currTarget += 1; // move on to next target
				conditionTimes.Add(currTimerValue);
			}

			// add times to all times and clear for the next condition
			List<float> tmpList = new List<float>(conditionTimes);
			allTimes.Add(tmpList);
			conditionTimes.Clear();

			// add noticeabiliy times to all times and clear for the next condition
			List<float> tmpList2 = new List<float>(noticeabilityTimes);
			allNoticeabilityTimes.Add(tmpList2);
			noticeabilityTimes.Clear();

			// pause for TLX
			// pause = true;
			// contButton.SetActive(true);
			// yield return new WaitUntil(() => pause == false);
			// contButton.SetActive(false);

		}

		// final message after all conditions
		instructionTxt.SetActive(true);
		displayText("Thank you for participating!");

		// write results to csv named result.csv
		DateTime now = DateTime.Now; // current date and time


		// adjust allTimes to get acquired time 
		// for (int m=0; m<3; m++) {
		// 	List<float> tmp = new List<float>(allTimes);

		// 	for (int k=0; k<allTimes[0].Count; k++) {
		// 		allTimes[m][k] -= allNoticeabilityTimes[m][k];
		// 	}
		// }

		for (int j=0; j<3; j++) {
			csv.Append("condition ");
			csv.Append(conditionsRan[j].ToString());
			csv.Append(",");
			foreach (float i in allTimes[j]) {
				csv.Append(i);
				csv.Append(",");
			}

			csv.Append("Noticeability Times");
			csv.Append(",");
			foreach(float k in allNoticeabilityTimes[j]) {
				csv.Append(k);
				csv.Append(",");
			}
		}
		csv.Append(",");
		csv.Append(now);
		csv.AppendLine(" ");
		try {
			File.AppendAllText("./result.csv", csv.ToString());
		} catch {
			File.AppendAllText("./BACKUPresult.csv", csv.ToString());
		}
    }

	public void continueGame() {
		print("CLICKING");
		pause = false;
	}

	void secondaryTask() {
		print("second task started");
		secondaryTaskDone = false;

		conditionTimes.Add(currTimerValue);
	}

	void recordHitTime() {

	}

	void displayText(string msg) {
		instructionTxt.GetComponent<Text>().text = msg;
	}



	// Update is called once per frame
	void Update () {
		RaycastHit TheHit;

		if (!secondaryTaskDone) {
			currTimerValue += Time.deltaTime;
			if (!recordNoticeability) {
				if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out TheHit)){
					// print(TheHit.transform.name);
					if (TheHit.transform.name == ("s" + tmpOrder[currTarget].ToString())  && (Input.GetMouseButtonDown(0))) {
						print("HIT TARGET BOX");
						secondaryTaskDone = true;
					} else if (Input.GetMouseButtonDown(0)) {
						conditionTimes.Add(-1.0f);
					}
				}
			}
		}

		if (recordNoticeability) {
			noticeabilityTimeValue += Time.deltaTime;
		}

		if (Input.GetKeyDown(KeyCode.Return))
        {
            continueGame();
        }

		if (Input.GetKeyDown(KeyCode.Space) && recordNoticeability) {
			// stop recording and turn off primary task 
			recordNoticeability = false;
			noticeabilityTimes.Add(noticeabilityTimeValue);

			print("recorded noticeability, now recording acquisition time");

			preGameObj.SetActive(false); // turn off object of pregame 	
		}
	}	
}
