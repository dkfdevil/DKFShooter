using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This script manages the playerlist
/// 
/// This is attached to the GameManager
/// This acceses by the PlayerName script
/// this script is accesed by the HealthandDamage script
/// This script is depended on the PlayerDataClass
/// </summary>

public class PlayerDataBaseScript : MonoBehaviour {
	
	//Variables Start
	
	public List<PlayerDataClass> PlayerList = new List<PlayerDataClass>();
	
	//This is used to add the player to the list in the first place
	public NetworkPlayer networkPlayer;
	
	//These are used to update the player list with the name of the player
	public bool nameSet = false;
	public string playerName;
	
	//These are used to update the player list with score of the player
	public bool scored = false;
	public int playerScore;
	
	//These are used to update the player list with the players chosen team
	public bool joinedTeam = false;
	public string playerTeam;
	
	//Variables End

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(nameSet == true)
		{
			//Edit the players record in the list and add their name
			networkView.RPC ("EditPlayerListWithName", RPCMode.AllBuffered, Network.player, playerName);
			nameSet = false;
		}
		if(scored == true)
		{
			//Edit the players record in the list and add their score
			networkView.RPC ("EditPlayerListWithScore", RPCMode.AllBuffered, Network.player, playerScore);
			scored = false;
		}
		if(joinedTeam == true)
		{
			//Edit the players record in the list and add their team
			networkView.RPC ("EditPlayerListWithTeam", RPCMode.AllBuffered, Network.player, playerTeam);
			joinedTeam = false;
		}
	}
	
	void OnPlayerConnected(NetworkPlayer networkPlayer)
	{
		//Add the player to the list. This is executed on the server
		networkView.RPC ("AddPlayerToList", RPCMode.AllBuffered, networkPlayer);
	}
	
	void OnPlayerDisconnected(NetworkPlayer networkPlayer)
	{
		//Remove the player from the list. This is executed on the server
		networkView.RPC ("RemovePlayerFromList", RPCMode.AllBuffered, networkPlayer);
	}
	
	[RPC]
	void AddPlayerToList(NetworkPlayer networkPlayer)
	{
		//Create a new entry in the PlayerList and supply the players network ID as the first enty
		PlayerDataClass playerData = new PlayerDataClass();
		playerData.networkPlayer = int.Parse(networkPlayer.ToString());
		PlayerList.Add(playerData);
	}
	
	[RPC]
	void RemovePlayerFromList(NetworkPlayer networkPlayer)
	{
		//Remove the entry in the PlayerList based on their network ID
		//But first we need to find the player in the list
		for(int i = 0; i < PlayerList.Count; i++)
		{
			if(PlayerList[i].networkPlayer == int.Parse(networkPlayer.ToString()))
			{
				PlayerList.RemoveAt(i);
			}
		}
	}
	
	[RPC]
	void EditPlayerListWithName(NetworkPlayer networkPlayer, string pName)
	{
		//Find the player in the playerlist based on their networkplayer ID
		//and add their name to their list
		for(int i = 0; i < PlayerList.Count; i++)
		{
			if(PlayerList[i].networkPlayer == int.Parse(networkPlayer.ToString()))
			{
				PlayerList[i].playerName = pName;
			}
		}
	}
	
	[RPC]
	void EditPlayerListWithScore(NetworkPlayer networkPlayer, int pScore)
	{
		//Find the player in the playerlist based on their networkplayer ID
		//and add their score to their list
		for(int i = 0; i < PlayerList.Count; i++)
		{
			if(PlayerList[i].networkPlayer == int.Parse(networkPlayer.ToString()))
			{
				PlayerList[i].playerScore = pScore;
			}
		}
	}
	
	[RPC]
	void EditPlayerListWithTeam(NetworkPlayer networkPlayer, string pTeam)
	{
		//Find the player in the playerlist based on their networkplayer ID
		//and add their team to their list
		for(int i = 0; i < PlayerList.Count; i++)
		{
			if(PlayerList[i].networkPlayer == int.Parse(networkPlayer.ToString()))
			{
				PlayerList[i].playerTeam = pTeam;
			}
		}
	}
}
