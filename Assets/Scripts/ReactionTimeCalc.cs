using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Leap.Unity;
using UnityEngine.Collections;
using Valve.VR;
using Leap.Unity.Attributes;
using LeapInternal;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;


public class ReactionTimeCalc : MonoBehaviour {

	
	public  float reactionTimer;
	public  bool isTiming = false;
	public  ExtendedFingerDetector Other;

	
//	int fingercount;
	
//	private bool Durum = false;
	
	
	//private int[] handnumber = new int[10];
		 string[] digits = new string[5];
	
	// Use this for initialization
	
	
	
		public void ColorActivate()
		{
			GetComponent<Renderer>().material.color = Color.blue;
			isTiming = false;
			Debug.Log("Reaction time: "+reactionTimer+"ms");
			reactionTimer = 0; //Reset timer
		}
    
		public void ColorDeactivate()
		{
		GetComponent<Renderer>().material.color = Color.red;
		}
	
		
		IEnumerator  Bekleme()
		{
			
			yield return new WaitForSeconds(2.5f);
			GetComponent<Renderer>().material.color = Color.green;
		//	Debug.Log("Reaction time: "+reactionTimer+"ms");
			yield return new WaitForSeconds(0.5f);

			int randomNumber = Random.Range(0, 4);

			GameObject.Find("Numerator").GetComponent<TextMesh>().text = digits[randomNumber];
			
			DigitDetector();
			
		//	int response = 0;

			
/*
			
			for (int i = 0; response != 1 && response != 2 && response != 3 && response != 4 && response != 5; i++)
			{
				
				Condition01();
				yield return new WaitForSeconds(0.10f);
				Condition02();
				yield return new WaitForSeconds(0.10f);
				Condition03();
				yield return new WaitForSeconds(0.10f);
				Condition04();
				yield return new WaitForSeconds(0.10f);
				Condition05();
				yield return new WaitForSeconds(0.10f);
				print(fingercount);
				response = fingercount;
				
			}

*/

			//	Debug.Log("Reaction time: "+reactionTimer+"ms");
			//Show digit
		}


	void DigitDetector() //Hand Digit Calculator
	{
		//BOTH hands are active. LOOK
		
		int a = 0,b = 0,c = 0,d = 0,e = 0,toplam = 0; 
		
		Leap.Hand hand;
		hand = Other.HandModel.GetLeapHand();

		if (hand.Fingers[0].IsExtended){ a = 1;}
		if (hand.Fingers[1].IsExtended){ b = 1;}
		if (hand.Fingers[2].IsExtended){ c = 1;}
		if (hand.Fingers[3].IsExtended){ d = 1;}
		if (hand.Fingers[4].IsExtended){ e = 1;}

		toplam= a + b + c + d + e;

		if (toplam == 1){ print(digits[0]);}
		if (toplam == 2){ print(digits[1]);}
		if (toplam == 3){ print(digits[2]);}
		if (toplam == 4){ print(digits[3]);}
		if (toplam == 5){ print(digits[4]);}
		
	}
	
	
	
	
	void Start ()
	{
	
		
		digits[0] = "1";
		digits[1] = "2";
		digits[2] = "3";
		digits[3] = "4";
		digits[4] = "5";
		
		GetComponent<Renderer>().material.color = Color.yellow;
		
		
	//	GameObject.FindWithTag("Player").GetComponent<ExtendedFingerDetector>().Thumb = PointingState.Extended;


		//print(digits.Length);
		//	print(digits[4]);

		/*	digits[5] = "6";
			digits[6] = "7";
			digits[7] = "8";
			digits[8] = "9";
			digits[9] = "10";
		*/
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (isTiming)
		{
			reactionTimer += Time.deltaTime;
		}
	
		if (Input.GetKeyDown(KeyCode.Space))
		{
			GetComponent<Renderer>().material.color = Color.grey;
			isTiming = true;
			StartCoroutine(Bekleme());
			
		}

	
		
		
		
		
	}
}
