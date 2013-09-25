using UnityEngine;
using System.Collections;

/// <summary>
/// This script is attached to the trigger gameobject on the player and
/// it manages the health of the player across the network and applies
/// damage to the player across the network
/// 
/// This script acceses the PlayerDatabase script to check the PlayerList
/// 
/// This script is accessed by the BlasterScript
/// 
/// SPECIAL NOTE
/// THE DMG AND HIT DETECTION PART IS CONTROLLED AND EXECUTED BY THE ATTACKING PLAYER
/// </summary>

public class HealthandDamageScript : MonoBehaviour {
	
	//Variables Start
	
	private GameObject parentObject;
	
	//Used in figuring out on whos computer the damage should be applied
	public string myAttacker;
	public bool iWasJustAttacked;
	
	//These variables are used in figuring out what the player has been hit by
	//and how much damage to apply	
	public bool hitByBlaster = false;
	private float blasterDamage = 30;
	
	//This is used to prevent the player from getting hit while they are undergoing destruction
	private bool destroyed = false;
	
	//These variables are used in managing the players health
	public float myHealth = 100;
	public float maxHealth = 100;
	private float healthRegenRate = 1.3f;
	
	//Variables End

	// Use this for initialization
	void Start () 
	{
		//The trigger gameobject is used in hit detection but it is
		//the parent that needs to be destroyed if the players health falls below zero
		parentObject = transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//If the player is hit by a opposing projectile then that projectile will 
		//have set iWasJustAttacked to true
		if(iWasJustAttacked == true)
		{
			GameObject multiPlayerManager = GameObject.Find("MultiPlayerManager");
			PlayerDataBaseScript playerDataBase = multiPlayerManager.GetComponent<PlayerDataBaseScript>();
			
			//Sift through the player list and only carry out hit detection if the attacking player is the one running this game instance
			for(int i = 0; i < playerDataBase.PlayerList.Count; i++)
			{
				if(myAttacker == playerDataBase.PlayerList[i].playerName)
				{
					if(int.Parse(Network.player.ToString()) == playerDataBase.PlayerList[i].networkPlayer)
					{
						//Check what the player was hit by and apply damage
						if(hitByBlaster == true && destroyed == false)
						{
							myHealth = myHealth - blasterDamage;
							//Send out an RPC so that this players attacker is updated 
							//This way the attacker can receive a score destroying the enemy player
							networkView.RPC("UpdateMyCurrentAttackerEverywhere",RPCMode.Others, myAttacker);
							
							//Send out an RPC so that this players health is reduced
							//Across the network
							networkView.RPC("UpdateMyCurrentHealthEverywhere",RPCMode.Others, myHealth);
						}
					}
				}
			}
			
			iWasJustAttacked = false;
			
			//Each player is responisble for their own destruction
			if(myHealth <= 0 && networkView.isMine == true)
			{
				//Remove this players RPC if we didnt do this a ghost of this player would remain in the game wich would be confusing
				Network.RemoveRPCs(Network.player);
				//Send out an rpc destroy our player across the network and for ourself
				networkView.RPC("DestroyMyselfEverywhere",RPCMode.All);
			}
			
			//Lets handle the health regen if myHealth falls below maxHealth
			if(myHealth < maxHealth)
			{
				myHealth = myHealth + healthRegenRate * Time.deltaTime;
			}
			//This can happen with the regen to cause it to be greater than the maxhealth
			//So we reset it here then
			if(myHealth > maxHealth)
			{
				myHealth = maxHealth;
			}
			
		}
	}
	
	[RPC]
	void UpdateMyCurrentAttackerEverywhere(string attacker)
	{
		myAttacker = attacker;
	}
	
	[RPC]
	void UpdateMyCurrentHealthEverywhere(float health)
	{
		myHealth = health;
	}
	
	[RPC]
	void DestroyMyselfEverywhere()
	{
		Destroy(parentObject);
	}
}
