using UnityEngine;
using System.Collections;

/// <summary>
/// This script is attached to the player and it
/// causes the camera to follow the camera head
/// </summary>

public class CameraHeadScript : MonoBehaviour {
	
	//Variables Start
	
	private Camera myCamera;
	private Transform cameraHeadTransform;
	
	//Variables End
	
	
	// Use this for initialization
	void Start () 
	{
		//Get the maincamera
		myCamera = Camera.main;
		//Get our own CameraHead
		cameraHeadTransform = transform.FindChild("CameraHead");
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Make the MainCamera follow the player cameraHeadTransform
		myCamera.transform.position = cameraHeadTransform.position;
		myCamera.transform.rotation = cameraHeadTransform.rotation;
	}
}
