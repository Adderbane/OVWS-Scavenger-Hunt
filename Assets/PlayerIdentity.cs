﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerIdentity : NetworkBehaviour {

	[SyncVar]
	public string myUsername;

	[SyncVar]
	public string myTeam;

	//NetworkInstanceId playerID;
	Transform myTransform;

	[SerializeField]
	public Material[] mats;

	public override void OnStartLocalPlayer ()
	{
		GetNetIdentity ();
		SetIdentity ();
		AssignMat();
	}

	// Use this for initialization
	void Awake () {
		myTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (myTransform.name == "" || myTransform.name == "Player(Clone)") {
			SetIdentity();
			AssignMat();
		}
	}

	[Client]
	void GetNetIdentity ()
	{
		CmdTellServerMyID (MakeMyID (), GetTeam());
	}


	string MakeMyID ()
	{
		string uniqueName = GameObject.Find ("NetManager").GetComponent<MyNetworkManager> ().username;
		return uniqueName;
	}

	public string GetTeam()
	{
		string team = GameObject.Find ("NetManager").GetComponent<MyNetworkManager> ().team;
		return team;
	}

	void AssignMat()
	{
		Renderer r = transform.Find("Capsule").GetComponent<Renderer>();
		if (myTeam == "red") {
			r.material = mats[0];
		}
		else {
			r.material = mats[1];
		}
	}

	[Command]
	void CmdTellServerMyID (string name, string team)
	{
		myUsername = name;
		myTeam = team;
	}

	[Client]
	void SetIdentity ()
	{
		if (!isLocalPlayer) {
			myTransform.name = myUsername;
		}
		else {
			myTransform.name = MakeMyID();
		}
	}

	public void PopupOn(string popText){
		if (isLocalPlayer) {
			GameObject.Find ("Popup").transform.GetChild(0).GetComponent<Text>().text = popText;
			GameObject.Find ("Popup").GetComponent<CanvasGroup> ().alpha = 1;
			GameObject.Find ("Popup").GetComponent<CanvasGroup> ().interactable = true;
		}
		
	}
	public void PopupOff(){
		GameObject.Find("Popup").GetComponent<CanvasGroup>().alpha = 0;
		GameObject.Find("Popup").GetComponent<CanvasGroup>().interactable = false;
	}

}
