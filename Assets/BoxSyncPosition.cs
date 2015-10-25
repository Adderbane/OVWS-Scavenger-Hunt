using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;

public class BoxSyncPosition : NetworkBehaviour
{


	[SyncVar (hook = "SyncPositionValues")]
	private Vector3
		syncPos;

	//variables to only send data when it's changed beyond a threshold.
	private Vector3 lastPosition;
  private float positionThreshold = 0.01f;
	private List<Vector3> syncPosList = new List<Vector3> ();


	void Start ()
	{


		//spawned = false;
	}

	// Update is called once per frame but it's not guaranteed to be exactly regular,
	// because the processing load varies.
	// This is why we typically use Time.deltaTime to smooth out transforms.
	// But it's a good place to interpolate (lerp) between an old position (
	// or rotation) and the newly acquired position information
	void Update ()
	{

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
	void SyncPositionValues (Vector3 latestPos)
	{
		syncPos = latestPos;
		syncPosList.Add (syncPos);
	}




	[Client]
	void TransmitPosition ()
	{
		// This is where we (the client) send out our position.

			if (Vector3.Distance (lastPosition, transform.position) > positionThreshold) {
				// Send a command to the server to update our position, and
				// it will update a SyncVar, which then automagically updates on everyone's game instance
				CmdSendPositionToServer (transform.position);
				lastPosition = transform.position;
			}

	}

}
