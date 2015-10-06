using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerIdentity : NetworkBehaviour {

	[SyncVar]
	public string myUsername;

	//NetworkInstanceId playerID;
	Transform myTransform;

	public override void OnStartLocalPlayer ()
	{
		GetNetIdentity ();
		SetIdentity ();
	}

	// Use this for initialization
	void Awake () {
		myTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (myTransform.name == "" || myTransform.name == "Player(Clone)") {
			SetIdentity();
		}
	}

	[Client]
	void GetNetIdentity ()
	{
		//playerID = GetComponent < NetworkIdentity> ().netId;
		CmdTellServerMyID (MakeMyID ());
	}

	string MakeMyID ()
	{
		string uniqueName = GameObject.Find ("NetManager").GetComponent<MyNetworkManager> ().username;
		return uniqueName;
	}

	[Command]
	void CmdTellServerMyID (string name)
	{
		myUsername = name;

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
}
