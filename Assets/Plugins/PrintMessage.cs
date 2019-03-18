using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Leap;
using Leap.Unity;

public class PrintMessage : MonoBehaviour
{

	public string Handname;

	public void PrintActivateMessage()
	{
		print("Activated");
		
		// if ( activated hand is this ){
		// count}
		// else if (){}
		
	}
    
	public void PrintDeactivateMessage()
	{
		print("Deactivated");
	}

	public GameObject ball;
	
	// Use this for initialization
	void Start () {
		
		PrintHandName(Handname);
		
	}
	
	// Update is called once per frame
	void Update (){
	
	}

	void PrintHandName(string Handname)
	{
		print("Current hand is: " + Handname);
	}
	
	
	void OnEnable()
	{

		print("inside OnEnable");

		

	}

	void OnDisable(){
		print("inside OnDisable");
	}
	
	

	
}


