using UnityEngine;
using System.Collections;


/// <summary>
/// This script is attached to the blaster projectile
/// and it handles the behavior of the projectile
/// 
/// This script is accesses by fireblasterscript to instantiate a new projecitle and give it a owner and team
/// This script accesses the healthanddamagescript to inform it has been attacked
/// </summary>


public class BlasterProjectileScript : MonoBehaviour {
	
	//Variables Start
	
	//Attached in the inspector
	public GameObject blasterProjectileExplosion;
	private Transform myTransform;
	private float projectileSpeed = 10;
	private bool hasCollided = false;
	private RaycastHit hit;
	private float rayRange = 1.5f;
	private float expireTime = 2;
	
	//Used for hit detection
	
	public string team;
	public string myOwner;
	
	//Variables End

	// Use this for initialization
	void Start () 
	{
		myTransform = transform;
		StartCoroutine(DestroyMyselfAfterSomeTime());
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Move the projectile up times the speed and times the fps time by tranlating it
		myTransform.Translate(Vector3.up * projectileSpeed * Time.deltaTime);
		//Check collision with the projectile
		if(Physics.Raycast(myTransform.position,myTransform.up, out hit, rayRange) && hasCollided == false)
		{
			//If the collider has the tag floor then
			if(hit.transform.tag == "Floor")
			{
				
				//Create a explosion effect
				Instantiate(blasterProjectileExplosion, hit.point, Quaternion.identity);
				
				hasCollided = true;
				
				//Make the projectile invisible
				myTransform.renderer.enabled = false;
			}
			//If we collided with a player
			if(hit.transform.tag == "BlueTeamCollider" || hit.transform.tag == "RedTeamCollider")
			{
				//Create a explosion effect
				Instantiate(blasterProjectileExplosion, hit.point, Quaternion.identity);
				
				hasCollided = true;
				
				//Make the projectile invisible
				myTransform.renderer.enabled = false;
				
				//Acces the healthanddamagescript of the enemy player
				//and inform them that they have been attacked and by whom
				if(hit.transform.tag == "BlueTeamCollider" && team == "red")
				{
					HealthandDamageScript healthAndDamageScript = hit.transform.GetComponent<HealthandDamageScript>();
					healthAndDamageScript.iWasJustAttacked = true;
					healthAndDamageScript.myAttacker = myOwner;
					healthAndDamageScript.hitByBlaster = true;
				}
				if(hit.transform.tag == "RedTeamCollider" && team == "blue")
				{
					HealthandDamageScript healthAndDamageScript = hit.transform.GetComponent<HealthandDamageScript>();
					healthAndDamageScript.iWasJustAttacked = true;
					healthAndDamageScript.myAttacker = myOwner;
					healthAndDamageScript.hitByBlaster = true;
				}
			}
			
		}
	}
	
	IEnumerator DestroyMyselfAfterSomeTime()
	{
		//Wait for the timer to count up to the expiretime
		//Then destroy myself
		yield return new WaitForSeconds(expireTime);
		//Create a explosion effect
		//Only if we didnt collide
		if(hasCollided == false)
		{
			Instantiate(blasterProjectileExplosion, myTransform.position, Quaternion.identity);
		}
		Destroy(myTransform.gameObject);
	}
}
