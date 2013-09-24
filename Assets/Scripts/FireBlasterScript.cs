using UnityEngine;
using System.Collections;

/// <summary>
/// This script is attached to the player and allows
/// them to fire the blasterprojectile
/// </summary>

public class FireBlasterScript : MonoBehaviour {
	
	//Variables Start
	
	//Attached in the inspector
	public GameObject blasterProjectile;
	private Transform myTransform;
	private Transform cameraHeadTransform;
	private Vector3 launchPosition = new Vector3();
	private float fireRate = 0.5f;
	private float nextFire = 0;
	
	//Variables End
	
	// Use this for initialization
	void Start () 
	{
		myTransform = transform;
		cameraHeadTransform = transform.FindChild("CameraHead");	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetButton("FireWeapon") && Time.time > nextFire)
		{
			//Make sure that we update our time
			nextFire = Time.time + fireRate;
			
			//Setup our launch position just infront of our camerahead
			launchPosition = cameraHeadTransform.TransformPoint(0,0,0.2f);
			
			//Create our blasterprojectile and let it handle itself by BlasterProjectileScript
			//Also tilt its horizontal using the angle eulerAngles.x + 90
			Instantiate(blasterProjectile,launchPosition,Quaternion.Euler(cameraHeadTransform.eulerAngles.x + 90,myTransform.eulerAngles.y,0));
		}	
	}
}
