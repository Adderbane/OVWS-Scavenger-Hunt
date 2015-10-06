using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ChaseTarget : NetworkBehaviour {

	[SyncVar (hook="Move")]
	Vector3 pos;

	[SyncVar]
	string winner;

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

	void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.attachedRigidbody.gameObject.tag == "Player") {
			//collision.collider.attachedRigidbody.gameObject.GetComponent<FirstPersonController>().SendMessage ("GoFast");
			//string win = collision.collider.attachedRigidbody.gameObject.GetComponent<MyNetworkManager>().username;
			CmdCaught(); //win);
		}
	}

	[Command]
	void CmdCaught() //string newWin)
	{
		print ("caught");
		Vector3 newPos = new Vector3 (Random.Range(-150.0f, 150.0f), 0.0f, Random.Range(-150.0f, 150.0f));
		float height = Terrain.activeTerrain.SampleHeight(newPos) + 1.0f;
		newPos.y = height;
		pos = newPos;
		//winner = newWin;
	}

	public string GetWinner()
	{
		return winner;
	}
}
