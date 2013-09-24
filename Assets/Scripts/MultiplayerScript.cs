using UnityEngine;
using System.Collections;

/// <summary>
/// This script is attached to the MultiplayerManager and it
/// is the foundation for our multiplayer sytem.
/// </summary>

public class MultiplayerScript : MonoBehaviour {
	
	//Variables Start
	private string titleMessage = "DKFShooter In-Game";
	private string connectToIp = "127.0.0.1";
	private int connectToPort = 26500;
	private bool useNAT = false;
	private string ipAddress;
	private string port;
	private int numberOfPlayersConnected = 10;
	public string playerName;
	public string serverName;
	public string serverNameForClient;
	private bool iWantToSetupAServer = false;
	private bool iWantToConnectToAServer = false;
	
	//GUI PART
	private Rect connectionWindowRect;
	private int connectionWindowWidth = 400;
	private int connectionWindowHeight = 280;
	private int buttonHeigth = 60;
	private int leftIndent;
	private int topIndent;
	
	
	//Variables End

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void ConnectWindow(int windowID)
	{
		//Leave a gap from the header
		GUILayout.Space
	}	
}
