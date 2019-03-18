using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;


public class ReactionTimeCalc : MonoBehaviour {

	public  float reactionTimer;
	public  bool isTiming = false;
	
	// Use this for initialization
	
	
	
		public void ColorActivate()
		{
			GetComponent<Renderer>().material.color = Color.green;
			isTiming = false;
			Debug.Log("Reaction time: "+reactionTimer+"ms");
			reactionTimer = 0; //Reset timer
		}
    
		public void ColorDeactivate()
		{
		//GetComponent<Renderer>().material.color = Color.red;
		}
	
	
	void Start ()
	{
		GetComponent<Renderer>().material.color = Color.black;
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
			GetComponent<Renderer>().material.color = Color.blue;
			isTiming = true;
			
		}

		
		
		
		
	}
}
