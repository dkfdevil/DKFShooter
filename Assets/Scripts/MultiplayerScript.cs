using UnityEngine;
using System.Collections;

/// <summary>
/// This script is attached to the MultiplayerManager and it
/// is the foundation for our multiplayer sytem.
/// 
/// This script is acceses by the cursorcontrolscript
/// </summary>

//TODO: Refactor this multiplayerscript
//Split it up maybe into a client/server/gui
public class MultiplayerScript : MonoBehaviour {
	
	//Variables Start
	
	private string titleMessage = "DKFShooter In-Game";
	private bool iWantToSetupAServer = false;
	private bool iWantToConnectToAServer = false;
	
	//Server part
	private bool useNAT = false;
	private int serverPort = 26500;
	private string ipAddress;
	private string port;
	public string serverName;
	private int numberOfPlayers = 10;
	
	//Client part
	private string connectToIp = "127.0.0.1";
	private int connectToPort = 26500;
	public string playerName;
	public string serverNameForClient;
	
	//GUI PART
	//Mainwindow
	private Rect mainWindowRect;
	private int mainWindowWidth = 400;
	private int mainWindowHeight = 280;
	private int buttonHeigth = 60;
	private int mainWindowLeftIndent;
	private int mainWindowTopIndent;
	
	//Serverwindow
	private Rect serverWindowRect;
	private int serverWindowWidth = 400;
	private int serverWindowHeight = 280;
	private int serverWindowLeftIndent = 10;
	private int serverWindowTopIndent = 10;
	
	//clientwindow
	private Rect clientWindowRect;
	private int clientWindowWidth = 400;
	private int clientWindowHeight = 280;
	private int clientWindowLeftIndent = 10;
	private int clientWindowTopIndent = 10;
	public bool clientWindowShow = false;
	
	//Variables End

	// Use this for initialization
	void Start () 
	{
		//Load our playerPrefs
		//Load our last used serverName from registry otherwise default
		serverName = PlayerPrefs.GetString("serverName");
		if(serverName == "")
		{
			serverName = "Server";
		}
		//Load our last used playerName from registry otherwise default
		playerName = PlayerPrefs.GetString("playerName");
		if(playerName == "")
		{
			playerName = "Player";
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Client part
		if(Network.peerType == NetworkPeerType.Client)
		{
			if(Input.GetKeyDown(KeyCode.Escape))
			{
				clientWindowShow = !clientWindowShow;
			}
		}
	}
	
	void MainWindow(int windowID)
	{
		//Leave a gap from the header
		GUILayout.Space(15);
		
		//When the player launches the game they have the option
		//To create a server or join a server.
		//iWantToSetupAServer and iWantToConnectToAServer start as false
		//so the player has a option between a setup server and connect server		
		if(iWantToSetupAServer == false && iWantToConnectToAServer == false)
		{
			if(GUILayout.Button("Setup a server", GUILayout.Height(buttonHeigth)))
			{
				iWantToSetupAServer = true;
			}
			
			GUILayout.Space(10);
			
			if(GUILayout.Button("Connect to a server", GUILayout.Height(buttonHeigth)))
			{
				iWantToConnectToAServer = true;
			}
			
			GUILayout.Space(10);
			
			if(Application.isEditor == false && Application.isWebPlayer == false)
			{
				if(GUILayout.Button("Exit game", GUILayout.Height(buttonHeigth)))
				{
					//Application.quit only works for a standalone
					Application.Quit();
				}
			}
		}
		
		//Setup a server
		if(iWantToSetupAServer == true)
		{
			//The user can type a name for their server and into the textfield
			GUILayout.Label("Enter a name for your server");
			serverName = GUILayout.TextField(serverName);
			
			GUILayout.Space(5);
			
			//The user can type in the port number into the textfield, We defined the default value
			GUILayout.Label("Server Port");
			serverPort = int.Parse(GUILayout.TextField(serverPort.ToString()));
			
			GUILayout.Space(10);
			
			if(GUILayout.Button("Start Server", GUILayout.Height(buttonHeigth)))
			{
				//Create the server
				Network.InitializeServer(numberOfPlayers, serverPort, useNAT);
				
				//Save the servername using PlayerPrefs
				PlayerPrefs.SetString("serverName",serverName);
				
				iWantToSetupAServer = false;
			}
			
			GUILayout.Space(5);
			
			//We need a go back button
			if(GUILayout.Button("Go Back", GUILayout.Height(buttonHeigth)))
			{
				iWantToSetupAServer = false;
			}
		}
		
		//Connect to a server
		if(iWantToConnectToAServer == true)
		{
			//The user can setup their playername here
			GUILayout.Label("Enter your player name");
			playerName = GUILayout.TextField(playerName);
			
			GUILayout.Space(5);
			
			//The user can type the ip address to connect to that server
			GUILayout.Label("Enter server ip address");
			connectToIp = GUILayout.TextField(connectToIp);
			
			GUILayout.Space(5);
			
			//the user can type the port number to connect to that server
			GUILayout.Label("Enter server port");
			connectToPort = int.Parse(GUILayout.TextField(connectToPort.ToString()));
			
			GUILayout.Space(5);
			
			//Lets connect to the server
			if(GUILayout.Button("Connect", GUILayout.Height(buttonHeigth)))
			{
				//Lets ensure that a player cant join a server with a empty name
				if(playerName == "")
				{
					playerName = "Player"; 
				}
				
				Network.Connect(connectToIp,connectToPort);
				
				//Also save the playername
				PlayerPrefs.SetString("playerName",playerName);				
			}
			
			GUILayout.Space(5);
			
			//We need a go back button
			if(GUILayout.Button("Go Back", GUILayout.Height(buttonHeigth)))
			{
				iWantToConnectToAServer = false;
			}
		}
	}
	
	void ServerWindow(int windowID)
	{
		GUILayout.Label("Server name: " + serverName);
		
		//Show the number of player connected
		GUILayout.Label("Number of players connected: " + Network.connections.Length);
		
		//Average ping of all players connected
		//If there is atleast 1 connection
		if(Network.connections.Length >= 1)
		{
			GUILayout.Label("Average ping: " + Network.GetAveragePing(Network.connections[0]));
		}
		else
		{
			GUILayout.Label("Average ping: N/A");
		}
		
		GUILayout.Space(5);
		
		//Shutdown the server if the user presses this button
		if(GUILayout.Button("Shutdown Server", GUILayout.Height(buttonHeigth)))
		{
			Network.Disconnect();
		}
	}
	
	void ClientWindow(int windowID)
	{
		//Show the player the server they are connected to and the average ping of the their connection
		GUILayout.Label("Connected to server: " + serverNameForClient);
		
		//Show the player average ping
		GUILayout.Label("Average ping: " + Network.GetAveragePing(Network.connections[0]));
		
		GUILayout.Space(5);
		
		//Allow the player to disconnect from the server when they press this button
		if(GUILayout.Button("Disconnect", GUILayout.Height(buttonHeigth)))
		{
			Network.Disconnect();
		}
		
		GUILayout.Space(5);
		
		//Allow the player to return to the game
		//Is also needed cause the webplayer uses the ESC key to exit fullscreen
		if(GUILayout.Button("Return to the game", GUILayout.Height(buttonHeigth)))
		{
			clientWindowShow = false;
		}	
	}
	
	void OnDisconnectedFromServer()
	{
		//Client-Server Side
		//If we are disconnected from the server
		//Or if we lost the connection then reset the level and also all variables by restarting
		Application.LoadLevel(Application.loadedLevel);
		print("OnDisconnectedFromServer");
	}
	
	void OnPlayerDisconnected(NetworkPlayer networkPlayer)
	{
		//ServerSide
		//When the player leaves the server delete them across the network
		//Along with their rpcs so that other players no longer see them
		Network.RemoveRPCs(networkPlayer);
		Network.DestroyPlayerObjects(networkPlayer);
		print("OnPlayerDisconnected");
	}
	
	void OnPlayerConnected(NetworkPlayer networkPlayer)
	{
		//Tell our new connected player our servername
		networkView.RPC("TellPlayerServerName", networkPlayer, serverName);
	}
	
	void OnGUI()
	{
		//If the player is disconnected then run the MainWindow function
		if(Network.peerType == NetworkPeerType.Disconnected)
		{
			//Determine the position of the window based on the width and height of the screen
			//So we can reposition and resize so it will be in the middle
			mainWindowLeftIndent = Screen.width / 2 - mainWindowWidth / 2;
			mainWindowTopIndent = Screen.height / 2 - mainWindowHeight / 2;
			
			mainWindowRect = new Rect(mainWindowLeftIndent,mainWindowTopIndent,mainWindowWidth,mainWindowHeight);
			mainWindowRect = GUILayout.Window(0,mainWindowRect, MainWindow, titleMessage);
		}
		//If we are running as a server lets run the serverwindow
		if(Network.peerType == NetworkPeerType.Server)
		{
			serverWindowRect = new Rect(serverWindowLeftIndent,serverWindowTopIndent,serverWindowWidth,serverWindowHeight);
			serverWindowRect = GUILayout.Window(1,serverWindowRect, ServerWindow, titleMessage);
		}
		//If we are running as a client lets run the clientwindow
		if(Network.peerType == NetworkPeerType.Client)
		{
			//Lets show the clientWindow
			if(clientWindowShow == true)
			{
				clientWindowRect = new Rect(clientWindowLeftIndent,clientWindowTopIndent,clientWindowWidth,clientWindowHeight);
				clientWindowRect = GUILayout.Window(2,clientWindowRect, ClientWindow, titleMessage);
			}
		}
	}
	
	//Used to tell the multiplayerscript in connected players the servername
	//Otherwise players connecting cant tell the servername
	//Server-->Client
	[RPC]
	void TellPlayerServerName(string servername)
	{
		serverNameForClient = servername;
	}
}
