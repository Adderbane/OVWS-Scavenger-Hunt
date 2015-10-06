using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ChaseTarget : NetworkBehaviour {

	[SyncVar (hook="Move")]
	Vector3 pos;

	[SyncVar (hook="SetWinner")]
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
			string win = collision.collider.attachedRigidbody.gameObject.GetComponent<PlayerIdentity>().myUsername;
			CmdCaught(win);
		}
	}

	[Command]
	void CmdCaught (string newWin)
	{
		Vector3 newPos = new Vector3 (Random.Range(-150.0f, 150.0f), 0.0f, Random.Range(-150.0f, 150.0f));
		float height = Terrain.activeTerrain.SampleHeight(newPos) + 1.0f;
		newPos.y = height;
		pos = newPos;
		winner = newWin;
		GameObject.Find ("LastWin").GetComponent<Text> ().text = "Last Winner: " + GetWinner();
	}

	public string GetWinner()
	{
		return winner;
	}

	[Client]
	void SetWinner (string nextWin)
	{
		GameObject.Find ("LastWin").GetComponent<Text> ().text = "Last Winner: " + nextWin;
	}
}
