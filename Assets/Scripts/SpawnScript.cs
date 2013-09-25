using UnityEngine;
using System.Collections;

/// <summary>
/// This is script is attached to the SpawnManager and it allows
/// the player to spawn into the multiplayer game
/// 
/// this script is accesed by the fireblaster script to determine wich team the player is on
/// </summary>

public class SpawnScript : MonoBehaviour {
	
	//Variables Start
	
	//Used to determine if the player needs to spawn into the game
	private bool justConnectedToServer = false;
	//Used to determine wich team the player is on
	public bool amIOnTheRedTeam = false;
	public bool amIOnTheBlueTeam = false;
	
	//Player prefabs are connected to these in the inspector
	public Transform redTeamPlayer;
	public Transform blueTeamPlayer;
	
	private int redTeamGroup = 0;
	private int blueTeamGroup = 1;
	
	//Used to capture spawn points
	private GameObject[] redSpawnPoints;
	private GameObject[] blueSpawnPoints;
	
	//Define the joinTeamWindow
	private Rect joinTeamWindowRect;
	private string joinTeamWindowTile = "Team Selection";
	private int joinTeamWindowWidth = 400;
	private int joinTeamWindowHeight = 280;
	private int buttonHeigth = 60;
	private int joinTeamWindowLeftIndent;
	private int joinTeamWindowTopIndent;
	
	
	//Variables End
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnConnectedToServer()
	{
		justConnectedToServer = true;
	}
	
	void JoinTeamWindow(int windowID)
	{
		//If the player clicks on the red team button then assign them to the red team
		if(GUILayout.Button("Join Red Team", GUILayout.Height(buttonHeigth)))
		{
			amIOnTheRedTeam = true;
			justConnectedToServer = false;
			SpawnPlayer("red");
		}
		//If the player clicks on the blue team button then assign them to the blue team
		if(GUILayout.Button("Join Blue Team", GUILayout.Height(buttonHeigth)))
		{
			amIOnTheBlueTeam = true;
			justConnectedToServer = false;
			SpawnPlayer("blue");
		}
	}
	
	void OnGUI()
	{
		//If the player has just connected to the server then draw the join team window
		if(justConnectedToServer == true)
		{
			//Determine the position of the window based on the width and height of the screen
			//So we can reposition and resize so it will be in the middle
			joinTeamWindowLeftIndent = Screen.width / 2 - joinTeamWindowWidth / 2;
			joinTeamWindowTopIndent = Screen.height / 2 - joinTeamWindowHeight / 2;
			
			joinTeamWindowRect = new Rect(joinTeamWindowLeftIndent,joinTeamWindowTopIndent,joinTeamWindowWidth,joinTeamWindowHeight);
			joinTeamWindowRect = GUILayout.Window(0,joinTeamWindowRect, JoinTeamWindow, joinTeamWindowTile);
		}
	}
	
	void SpawnPlayer(string team)
	{
		//Find all the spawn points for the selected team
		//And place them all in a array
		redSpawnPoints = GameObject.FindGameObjectsWithTag("SpawnRedTeam");
		blueSpawnPoints = GameObject.FindGameObjectsWithTag("SpawnBlueTeam");
		
		//Randomly select one of those spawn points
		GameObject randomSpawnPoint;
		if(team == "blue")
		{
			print("blue spawned");
			randomSpawnPoint = blueSpawnPoints[Random.Range(0,blueSpawnPoints.Length)];
			//Instantiate the player at the randomly selected spawn point
			Network.Instantiate(blueTeamPlayer, randomSpawnPoint.transform.position,randomSpawnPoint.transform.rotation, blueTeamGroup);
		}
		if(team == "red")
		{
			print("red spawned");
			randomSpawnPoint = redSpawnPoints[Random.Range(0,redSpawnPoints.Length)];
			//Instantiate the player at the randomly selected spawn point
			Network.Instantiate(redTeamPlayer, randomSpawnPoint.transform.position,randomSpawnPoint.transform.rotation, redTeamGroup);
		}
	}
}
