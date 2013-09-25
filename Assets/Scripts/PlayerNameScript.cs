using UnityEngine;
using System.Collections;

/// <summary>
/// This script incorporates the players name into the game.
/// and this script acceses the PlayerDatabase script to tell it
/// to add the players name to the playerlist
/// </summary>

public class PlayerNameScript : MonoBehaviour {
	
	//Variables Start
	
	public string playerName;
	
	//Variables End
	
	
	void Awake()
	{
		//When the player spawns into the game retrieve their name from PlayerPrefs
		//Also rename the gameoject to their respective name
		//Also update it accross the network
		//And check if the name is legit
		if(networkView.isMine == true)
		{
			//This cant be empty cause we checked it in our multiplayerscript
			//right before the connect to server button
			playerName = PlayerPrefs.GetString("playerName");
			//Lets apply some rules to their name
			//If there is a conflict then reassign their name to a random number
			
			foreach(GameObject objectNameCheck in GameObject.FindObjectsOfType(typeof(GameObject)))
			{
				if(playerName == objectNameCheck.name)
				{
					int x = Random.Range(0,1000);
					playerName = "(" + x.ToString() + ")";
					
					PlayerPrefs.SetString("playerName", playerName);
				}
			}
			//Update our playerdatabase
			UpdatePlayerDatabase(playerName);
			
			//Send out an RPC to ensure this players name is slapped onto their gameobject
			//This is important for hit detection
			networkView.RPC("UpdateMyNameEveryWhere", RPCMode.AllBuffered, playerName);
		}
		
	}
	
	void UpdatePlayerDatabase(string pName)
	{
		//Tell the playerdataase script to append this
		//Playersname to the list		
		GameObject multiplayerManager = GameObject.Find("MultiPlayerManager");
		
		PlayerDataBaseScript dataBase = multiplayerManager.GetComponent<PlayerDataBaseScript>();
		
		dataBase.nameSet = true;
		dataBase.playerName = pName;
	}
	
	[RPC]
	void UpdateMyNameEveryWhere(string pName)
	{
		//Change the players GameObject name to their actual player name
		gameObject.name = pName;
		
		playerName = pName;
	}
}
