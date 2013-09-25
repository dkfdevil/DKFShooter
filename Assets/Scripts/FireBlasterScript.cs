using UnityEngine;
using System.Collections;

/// <summary>
/// This script is attached to the player and allows
/// them to fire the blasterprojectile
/// 
/// This script acceses the spawnmanager to see on wich team it is
/// This script acceses the blasterprojectile of a newly projectile to give it a owner and a team
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
	
	//Used to determine wich team the player is on
	private bool iAmOnTheRedTeam = false;
	private bool iAmOnTheBlueTeam = false;
	
	//Variables End
	
	// Use this for initialization
	void Start () 
	{
		if(networkView.isMine == true)
		{
			myTransform = transform;
			cameraHeadTransform = transform.FindChild("CameraHead");
			
			//Find the spawnmanager
			//Acces the script to find out wich team we are on
			GameObject spawnManager = GameObject.Find("CameraHead");
			
			SpawnScript spawnScript = spawnManager.GetComponent<SpawnScript>();
			
			if(spawnScript.amIOnTheRedTeam == true)
			{
				iAmOnTheRedTeam = true;
			}
			if(spawnScript.amIOnTheBlueTeam == true)
			{
				iAmOnTheBlueTeam = true;
			}
		}
		else
		{
			enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Only fire if our cursor is locked
		if(Screen.lockCursor == false)
		{
			return;
		}
		
		if(Input.GetButton("FireWeapon") && Time.time > nextFire)
		{
			//Make sure that we update our time
			nextFire = Time.time + fireRate;
			
			//Setup our launch position just infront of our camerahead
			launchPosition = cameraHeadTransform.TransformPoint(0,0,0.2f);
			
			//Create our projectile across the network
			//Create our blasterprojectile and let it handle itself by BlasterProjectileScript
			//Also tilt its horizontal using the angle eulerAngles.x + 90
			if(iAmOnTheRedTeam == true)
			{
				networkView.RPC("SpawnBlasterProjectile",RPCMode.All,launchPosition,Quaternion.Euler(cameraHeadTransform.eulerAngles.x + 90,myTransform.eulerAngles.y,0),myTransform.name,"red");	
			}
			if(iAmOnTheBlueTeam == true)
			{
				networkView.RPC("SpawnBlasterProjectile",RPCMode.All,launchPosition,Quaternion.Euler(cameraHeadTransform.eulerAngles.x + 90,myTransform.eulerAngles.y,0),myTransform.name,"blue");		
			}
		}	
	}
	
	[RPC]
	void SpawnBlasterProjectile(Vector3 position, Quaternion rotation, string myOwner, string team)
	{
		//Acces the blasterscript on the newly instantiated blaster projectile
		//and supply the owner of the projectile and team
		GameObject projectile = Instantiate(blasterProjectile, position, rotation) as GameObject;
		BlasterProjectileScript projectileScript = projectile.GetComponent<BlasterProjectileScript>();
		projectileScript.myOwner = myOwner;
		projectileScript.team = team;
	}
}
