using UnityEngine;
using System.Collections;

/// <summary>
/// This script is attached to the player and it controls the cursor
/// So it can lock or unlock it
/// 
/// This script acceses the MultiplayerScript
/// </summary>

public class CursorControlScript : MonoBehaviour {
	
	//Variables Start
	//Needed to acces the multiplayerscript
	private GameObject multiplayerManager;
	private MultiplayerScript multiplayerScript;
	//Variables End
	

	// Use this for initialization
	void Start () 
	{
		if(networkView.isMine == true)
		{
			multiplayerManager = GameObject.Find("MultiPlayerManager");
			multiplayerScript = multiplayerManager.GetComponent<MultiplayerScript>();
		}
		else
		{
			enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(multiplayerScript.clientWindowShow == false)
		{
			//If there arent any windows then lock the cursor
			Screen.lockCursor = true;
		}
		if(multiplayerScript.clientWindowShow == true)
		{
			//If there arent any windows then lock the cursor
			Screen.lockCursor = false;
		}
	}
}
