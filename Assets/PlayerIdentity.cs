using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

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
		//playerID = GetComponent < NetworkIdentity> ().netId;
		CmdTellServerMyID (MakeMyID (), GetTeam());
	}


	string MakeMyID ()
	{
		string uniqueName = GameObject.Find ("NetManager").GetComponent<MyNetworkManager> ().username;
		return uniqueName;
	}

	string GetTeam()
	{
		string team = GameObject.Find ("NetManager").GetComponent<MyNetworkManager> ().team;
		return team;
	}

	void AssignMat()
	{
		Renderer r = transform.Find("Capsule").GetComponent<Renderer>();
		if (myTeam == "red") {
			r.material = mats[0];
			Vector3 newPos = new Vector3 (Random.Range(100.0f, 120.0f), 0.0f, Random.Range(-150.0f, -120.0f));
			float height = Terrain.activeTerrain.SampleHeight(newPos) + 1.4f;
			newPos.y = height;
			myTransform.position = newPos;
		}
		else {
			r.material = mats[1];
			Vector3 newPos = new Vector3 (Random.Range(-200.0f, -150.0f), 0.0f, Random.Range(100.0f, 175.0f));
			float height = Terrain.activeTerrain.SampleHeight(newPos) + 1.4f;
			newPos.y = height;
			myTransform.position = newPos;
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
}
