using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ChaseTarget : NetworkBehaviour {

	[SyncVar (hook="Move")]
	protected Vector3 pos;

	[SyncVar (hook="SetWinner")]
	protected string winner;

	ScoreKeep scoreKeep;

	// Use this for initialization
	void Start () {
		winner = "";
		scoreKeep = GameObject.Find ("ScoreKeeper").GetComponent<ScoreKeep> ();
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
			string team = collision.collider.attachedRigidbody.gameObject.GetComponent<PlayerIdentity>().myTeam;
			CmdCaught(team);

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
		scoreKeep.UpdateScore (newWin);
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
		//GameObject.Find ("LastWin").GetComponent<Text> ().text = "Last Winner: " + nextWin;
	}
}
