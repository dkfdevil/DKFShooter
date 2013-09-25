using UnityEngine;
using System.Collections;

/// <summary>
/// This script is attached to the player and it ensures that every players position, rotation, and scale
/// are kept up to date across the network
/// </summary>

public class MovementUpdateScript : MonoBehaviour {
	
	//Variables Start
	
	private Vector3 lastPosition;
	private Quaternion lastRotation;
	private Transform myTransform;
	
	//Variables End
	
	// Use this for initialization
	void Start () 
	{
		if(networkView.isMine == true)
		{
			myTransform = transform;
			//Ensure that everyone sees the player at the correct location
			//the moment they spawn
			networkView.RPC("UpdateMovement",RPCMode.OthersBuffered, myTransform.position, myTransform.rotation);
		}
		else
		{
			enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		//If the player has moved at all then fire off an RPC to update the players postion and rotation accross the network
		if(Vector3.Distance(myTransform.position, lastPosition) >= 0.1)
		{
			//Update our lastposition
			lastPosition = myTransform.position;
			networkView.RPC("UpdateMovement",RPCMode.OthersBuffered, myTransform.position, myTransform.rotation);
		}
		if(Quaternion.Angle(myTransform.rotation, lastRotation) >= 1)
		{
			//Update our lastrotation
			lastRotation = myTransform.rotation;
			networkView.RPC("UpdateMovement",RPCMode.OthersBuffered, myTransform.position, myTransform.rotation);
		}
	}
	
	[RPC]
	void UpdateMovement(Vector3 newPosition, Quaternion newRotation)
	{
		transform.position = newPosition;
		transform.rotation = newRotation;
	}
}
