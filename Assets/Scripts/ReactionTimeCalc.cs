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

	private int Condition01;
	
	
	
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
			
		//	Debug.Log("Reaction time: "+reactionTimer+"ms");
			//Show digit
		}


/*
	void DigitDetector()
	{
		if ()
		{
			print("1");

		} else if ()
		{
			print("2");
		}
		else
		{
			print("NO");
		}
		
	}
	
*/	
	
	
	void Start ()
	{

		if (Other.Thumb == PointingState.Extended && Other.MaximumExtendedCount == 1)
		{
			print("1");
		}
		else
		{
			print("NO");
		}
		
		
	
		
	// Condition 02 

		
	//	Other.Thumb = PointingState.Extended;
	//	Other.Index = PointingState.Extended;
	//	Other.MaximumExtendedCount = 2;
		
		
		
		
		
		GetComponent<Renderer>().material.color = Color.yellow;

		digits[0] = "1";
		digits[1] = "2";
		digits[2] = "3";
		digits[3] = "4";
		digits[4] = "5";

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
