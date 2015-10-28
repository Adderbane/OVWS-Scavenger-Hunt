﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ChaseTarget : NetworkBehaviour {

	[SyncVar (hook="Move")]
	protected Vector3 pos;

	protected ScoreKeep scoreKeep;

	public string popupText;
	public int score;
	
	protected bool isTouch;
	protected PlayerIdentity colliderId;

	// Use this for initialization
	void Start () {
		scoreKeep = GameObject.Find ("ScoreKeeper").GetComponent<ScoreKeep> ();
		popupText += "\n\n (Click to close)";
		GameObject.Find ("Popup").transform.GetChild (0).GetComponent<Text> ().text = popupText;
		isTouch = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (isTouch) {
			if(Input.GetKeyDown(KeyCode.L)){
				string team = colliderId.myTeam;
				Caught(team, score);
				colliderId.PopupOn(popupText);
			}
		}
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
			isTouch = true;
			colliderId = collision.collider.attachedRigidbody.gameObject.GetComponent<PlayerIdentity>();
		}
	}
	
	protected virtual void OnCollisionExit(Collision collision){
		if (collision.collider.attachedRigidbody.gameObject.tag == "Player") {
			isTouch = false;
			colliderId = null;
		}
	}

	[Client]
	protected virtual void Caught(string team, int score){
		string strScore = score.ToString ();
		scoreKeep.UpdateScore (team, strScore);
	}

	public void PopupOff(){
		GameObject.Find ("Popup").GetComponent<CanvasGroup> ().alpha = 0;
		GameObject.Find ("Popup").GetComponent<CanvasGroup> ().interactable = false;
	}

}
