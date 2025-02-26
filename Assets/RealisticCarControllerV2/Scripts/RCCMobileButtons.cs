//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using System;
using UnityEngine;
using System.Collections;

public class RCCMobileButtons : MonoBehaviour {

	public GameObject gasButton;
	public GameObject brakeButton;
	public GameObject leftButton;
	public GameObject rightButton;
	public GameObject steeringWheel;
	public GameObject handbrakeButton;
	public GameObject boostButton;

	public void ChangeCamera () {

		if(GameObject.FindObjectOfType<RCCCamManager>())
			GameObject.FindObjectOfType<RCCCamManager>().ChangeCamera();

	}


	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.W))
		{
			gasButton.GetComponent<RCCUIController>().ClickDown();
		}
		if (Input.GetKeyDown(KeyCode.A))
		{
			leftButton.GetComponent<RCCUIController>().ClickDown();
		}
		if (Input.GetKeyDown(KeyCode.S))
		{
			brakeButton.GetComponent<RCCUIController>().ClickDown();
		}
		if (Input.GetKeyDown(KeyCode.D))
		{
			rightButton.GetComponent<RCCUIController>().ClickDown();
		}
		
		if (Input.GetKeyUp(KeyCode.W))
		{
			gasButton.GetComponent<RCCUIController>().ClickUp();
		}
		if (Input.GetKeyUp(KeyCode.A))
		{
			leftButton.GetComponent<RCCUIController>().ClickUp();
		}
		if (Input.GetKeyUp(KeyCode.S))
		{
			brakeButton.GetComponent<RCCUIController>().ClickUp();
		}
		if (Input.GetKeyUp(KeyCode.D))
		{
			rightButton.GetComponent<RCCUIController>().ClickUp();
		}
	}
}
