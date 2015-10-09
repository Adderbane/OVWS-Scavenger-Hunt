﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ChaseTarget : NetworkBehaviour {

	[SyncVar (hook="Move")]
	protected Vector3 pos;

	[SyncVar (hook="SetWinner")]
	protected string winner;

	// Use this for initialization
	void Start () {
		winner = "";
	}
	
	// Update is called once per frame
	void Update () {

	}

	[Client]
	void Move(Vector3 newPos)
	{
		gameObject.transform.position = newPos;
	}

	//Call Caught command when player collides
    protected virtual void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.attachedRigidbody.gameObject.tag == "Player") {
			string win = collision.collider.attachedRigidbody.gameObject.GetComponent<PlayerIdentity>().myUsername;
			CmdCaught(win);
		}
	}

	//Called when collision critera hit
	[Command]
	protected virtual void CmdCaught (string newWin)
	{
		Vector3 newPos = new Vector3 (Random.Range(-150.0f, 150.0f), 0.0f, Random.Range(-150.0f, 150.0f));
		float height = Terrain.activeTerrain.SampleHeight(newPos) + 1.0f;
		newPos.y = height;
		pos = newPos;
		winner = newWin;
		GameObject.Find ("LastWin").GetComponent<Text> ().text = "Last Winner: " + GetWinner();
	}

	//Returns winner
	public string GetWinner()
	{
		return winner;
	}

	//Sets winner on clients
	[Client]
	void SetWinner (string nextWin)
	{
		GameObject.Find ("LastWin").GetComponent<Text> ().text = "Last Winner: " + nextWin;
	}
}
