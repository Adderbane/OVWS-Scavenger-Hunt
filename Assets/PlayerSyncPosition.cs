﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerSyncPosition : NetworkBehaviour
{
	[SerializeField]
	private bool
		useHistoricalLerping = true;

	[SyncVar (hook = "SyncPositionValues")]
	private Vector3
		syncPos;


	private float lerpRate;
	private float normalLerpRate = 70;
	private float fasterLerpRate = 100;

	private bool spawned;

	//variables to only send data when it's changed beyond a threshold.
	private Vector3 lastPosition;
	private float positionThreshold = 0.01f;
	private List<Vector3> syncPosList = new List<Vector3> ();


	
	void Start ()
	{
		lerpRate = normalLerpRate;

		spawned = false;
	}
	
	// Update is called once per frame but it's not guaranteed to be exactly regular,
	// because the processing load varies.
	// This is why we typically use Time.deltaTime to smooth out transforms.
	// But it's a good place to interpolate (lerp) between an old position (
	// or rotation) and the newly acquired position information
	void Update ()
	{
		if (!spawned && isLocalPlayer) {
			if (this.gameObject.GetComponent<PlayerIdentity>().GetTeam() == "red") {
				this.gameObject.transform.position = new Vector3 (-41.9f, 1.04f, 1060.9f);
				CmdSendPositionToServer(new Vector3 (-41.9f, 1.04f, 1060.9f));
			}
			else {
				this.gameObject.transform.position = new Vector3 (25.4f, 1.6f, 1052.2f);
				CmdSendPositionToServer(new Vector3 (25.4f, 1.6f, 1052.2f));
			}
			spawned = true;
		}
		LerpPosition ();
	}
	
	// FixedUpdate will fire at regular intervals, making it a good place
	// To send our regular position updates.
	void FixedUpdate ()
	{
		TransmitPosition ();
	}


	[Command]
	void CmdSendPositionToServer (Vector3 pos)
	{
		//runs on server, we call on client
		syncPos = pos;
	}
	


	
	[Client]
	void TransmitPosition ()
	{
		// This is where we (the client) send out our position.
		if (isLocalPlayer) {
			if (Vector3.Distance (lastPosition, transform.position) > positionThreshold) {
				// Send a command to the server to update our position, and 
				// it will update a SyncVar, which then automagically updates on everyone's game instance
				CmdSendPositionToServer (transform.position);
				lastPosition = transform.position;
			}
		}
	}

	
	[Client]
	void SyncPositionValues (Vector3 latestPos)
	{
		syncPos = latestPos;
		syncPosList.Add (syncPos);
	}

	
	void LerpPosition ()
	{
		//only on non-client characters, not us
		//smootly move from our old position data to our updated data we got from the server.
		if (!isLocalPlayer) {
			if (useHistoricalLerping) {
				HistoricalLerp ();
			} else {
				OrdinaryLerp ();
			}
		}
	}
	
	void OrdinaryLerp ()
	{
		transform.position = Vector3.Lerp (transform.position, syncPos, Time.deltaTime * lerpRate);

	}

	void HistoricalLerp ()
	{
		if (syncPosList.Count > 0) {
			transform.position = Vector3.Lerp (transform.position, syncPosList [0], 
			                                  		Time.deltaTime * lerpRate);
			//if we're getting really close to that point, delete it
			if (Vector3.Distance (transform.position, syncPosList [0]) < positionThreshold) {
				syncPosList.RemoveAt (0);
			}

			// if we don't have so many in the list, lerp faster
			if (syncPosList.Count > 10) {
				lerpRate = fasterLerpRate;
			} else {
				lerpRate = normalLerpRate;
			}
		}
	}
}








