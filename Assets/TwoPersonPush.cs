﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TwoPersonPush : ChaseTarget {

	//Number of players in contact with an object
	[SyncVar]
	int numRed;
	[SyncVar]
	int numBlue;

	public GameObject rollinRock;
	Rigidbody rb;

	//private Transform coolBeans;

	//public Vector3 velocity;
	//public Transform target;
	//public float speed;
	//


	// Use this for initialization
	void Start () {
		numRed = 0;
		numBlue = 0;
		rb = rollinRock.GetComponent<Rigidbody>();
		//coolBeans = transform.position.x;
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
		if (collision.collider.attachedRigidbody.gameObject.tag == "Player") { //whys is there a NULL reference err here?
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

		Vector3 myForward = transform.TransformDirection (Vector3.forward);
		rb.AddForce (myForward * 10.0f);

		pos = rb.gameObject.transform.position; //Tells it to update on client

		//--------------------------------------------------------------------------

		//transform.Translate (velocity * Time.deltaTime);
		//this.transform.position += Vector3.up;

		//transform.Translate (0, Time.deltaTime, 0, Space.Self);


		//ball.transform.Translate(Vector3.up * Time.deltaTime, Space.Self);
	}
}
