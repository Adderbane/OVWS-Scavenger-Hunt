using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TwoPersonCatch : ChaseTarget {

	//Number of players in contact with an object
	[SyncVar]
	int numRed;
	[SyncVar]
	int numBlue;

	// Use this for initialization
	void Start () {
		numRed = 0;
		numBlue = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (isServer) {
			if (numRed >= 2) {
				Caught("red", score);
			}
			else if (numBlue >= 2) {
				Caught("blue", score);
			}
		}
	}

	//Recognize people hitting an object
	protected void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.attachedRigidbody.gameObject.tag == "Player") {
			string team = collision.collider.attachedRigidbody.gameObject.GetComponent<PlayerIdentity>().myTeam;
			CmdEnter(team);
		}
	}

	//People leave an object
	void OnCollisionExit(Collision collision)
	{
		if (collision.collider.attachedRigidbody.gameObject.tag == "Player") {
			string team = collision.collider.attachedRigidbody.gameObject.GetComponent<PlayerIdentity>().myTeam;
			CmdLeave(team);
		}
	}

	//Called from CollisionEnter
	[Command]
	void CmdEnter(string team)
	{
		if (team == "red") {
			numRed++;
			Debug.Log(numRed);
		}
		else {
			numBlue++;
		}
	}

	//Called from collision leave
	[Command]
	void CmdLeave(string team)
	{
		if (team == "red") {
			numRed--;
			Debug.Log(numRed);
		}
		else {
			numBlue--;
		}
	}

	//Called on server when collision criteria are met
	[Client]
	protected override void Caught(string newWin, int score)
	{
		numRed = 0;
		numBlue = 0;
		Vector3 newPos = new Vector3 (Random.Range(-150.0f, 150.0f), 0.0f, Random.Range(-150.0f, 150.0f));
		float height = Terrain.activeTerrain.SampleHeight(newPos) + 1.0f;
		newPos.y = height;
		pos = newPos;
		Debug.Log(numRed);
	}
}
