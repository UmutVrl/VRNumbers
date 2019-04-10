using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using Leap;
using UnityEngine;
using Leap.Unity;
using UnityEngine.Collections;
using Valve.VR;
using Leap.Unity.Attributes;
using LeapInternal;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using Hand = Valve.VR.InteractionSystem.Hand;


public class ReactionTimeCalc : MonoBehaviour {

	
	public  float reactionTimer;
	public  bool isTiming = false;
//	public  ExtendedFingerDetector Other;
//	public  HandModelBase HandModel;
//	public  float Period = .1f;
	private Controller controller;
	private Frame current;
	private Leap.Hand handRight;
	
	
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
	
		private IEnumerator watcherCoroutine;
		
		private IEnumerator  BeklemeCongruent()
		{
			
			int randomNumber = Random.Range(0, 4);
			int randomNumberSecond = Random.Range(0, 4);
			
			
			yield return new WaitForSeconds(2.5f);
			GameObject.Find("Numerator").GetComponent<TextMesh>().text = digits[randomNumber];
			isTiming = true;
			yield return new WaitUntil(() => DigitDetector() == randomNumber+1 );
			Debug.Log(DigitDetector() + "  Reaction time: " + reactionTimer + " ms");
			GameObject.Find("Numerator").GetComponent<TextMesh>().text = "TRUE";
			yield return new WaitForSeconds(0.2f);
			GameObject.Find("Numerator").GetComponent<TextMesh>().text = "";
			isTiming = false;
			reactionTimer = 0;
			
			yield return new WaitForSeconds(2.5f);
			GameObject.Find("Numerator").GetComponent<TextMesh>().text = digits[randomNumberSecond];
			isTiming = true;
			yield return new WaitUntil(() => DigitDetector() == randomNumberSecond+1 );
			Debug.Log(DigitDetector() + "  Reaction timeSecond: " + reactionTimer + " ms");
			GameObject.Find("Numerator").GetComponent<TextMesh>().text = "TRUE";
			yield return new WaitForSeconds(0.2f);
			GameObject.Find("Numerator").GetComponent<TextMesh>().text = "";
			isTiming = false;
			reactionTimer = 0;

		}
	
		IEnumerator  BeklemeIncongruent()
		{
			int randomNumber = Random.Range(0, 4);
			int randomNumberSecond = Random.Range(0, 4);
			
			
			yield return new WaitForSeconds(2.5f);
			GameObject.Find("Numerator").GetComponent<TextMesh>().text = digits[randomNumber];
			isTiming = true;
//			GestureIncongruent();
			yield return new WaitUntil(() => DigitDetector() == randomNumber+1 );
			Debug.Log(DigitDetector() + "  Reaction time: " + reactionTimer + " ms");
			GameObject.Find("Numerator").GetComponent<TextMesh>().text = "TRUE";
			yield return new WaitForSeconds(0.2f);
			GameObject.Find("Numerator").GetComponent<TextMesh>().text = "";
			isTiming = false;
			reactionTimer = 0;
			
			yield return new WaitForSeconds(2.5f);
			GameObject.Find("Numerator").GetComponent<TextMesh>().text = digits[randomNumberSecond];
			isTiming = true;
			yield return new WaitUntil(() => DigitDetector() == randomNumberSecond+1 );
			Debug.Log(DigitDetector() + "  Reaction timeSecond: " + reactionTimer + " ms");
			GameObject.Find("Numerator").GetComponent<TextMesh>().text = "TRUE";
			yield return new WaitForSeconds(0.2f);
			GameObject.Find("Numerator").GetComponent<TextMesh>().text = "";
			isTiming = false;
			reactionTimer = 0;

		}

	
	int DigitDetector() //Hand Digit Calculator
	{
		
		int a = 0,b = 0,c = 0,d = 0,e = 0,sum = 0;
		
		//***BOTH hands should active. LOOK***
		current = controller.Frame();

		if (current.Hands.Count == 1)
		{
			if (current.Hands[0].IsRight)
				handRight = current.Hands[0];
			else
				handRight = null;
		}
		else if (current.Hands.Count == 2)
			if (current.Hands[0].IsRight)
				handRight = current.Hands[0];
			else
				handRight = current.Hands[1];
		else if (current.Hands.Count == 0)
			handRight = null;

		if (handRight != null)
		{
			if (handRight.Fingers[0].IsExtended){ a = 1;}
			if (handRight.Fingers[1].IsExtended){ b = 1;}
			if (handRight.Fingers[2].IsExtended){ c = 1;}
			if (handRight.Fingers[3].IsExtended){ d = 1;}
			if (handRight.Fingers[4].IsExtended){ e = 1;}
		}
		
		sum = a + b + c + d + e;
	//	handRight.Fingers[2].HandId.
		
		return sum;
	}

	void GestureIncongruent()
	{
		
		
	}
	
	
	void Start ()
	{
	
		controller = new Controller();
		current = controller.Frame();
		
		digits[0] = "1";
		digits[1] = "2";
		digits[2] = "3";
		digits[3] = "4";
		digits[4] = "5";
		
		GetComponent<Renderer>().material.color = Color.yellow;

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
			
			StartCoroutine(BeklemeCongruent());
			
		} else if (Input.GetKeyDown(KeyCode.A))
		{
			GetComponent<Renderer>().material.color = Color.grey;
			
			StartCoroutine(BeklemeIncongruent());
			
		}

	}
}
