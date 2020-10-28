using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pozit : MonoBehaviour {

	//movement speed in units per second
	private float movementSpeed = 5f;
	
	// Update is called once per frame
	void Update () {
		
		//get the Input from Horizontal axis
		transform.position = GameObject.Find("Main Camera").transform.position;
		transform.rotation = GameObject.Find("Main Camera").transform.rotation;
		
		//update the position
		//transform.position = transform.position + new Vector3(horizontalInput * movementSpeed * Time.deltaTime, verticalInput * movementSpeed * Time.deltaTime, 0);
		
		}
}
