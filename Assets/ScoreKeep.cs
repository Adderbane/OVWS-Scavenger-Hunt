using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Networking.NetworkSystem;

public class ScoreKeep : NetworkBehaviour {
	
	[SyncVar]
	public int redScore;
	[SyncVar]
	public int blueScore;
	
	const short chatMsg = MsgType.Highest + 2;
	
	Text redScoreText;
	Text blueScoreText;
	
	GameObject sphere;
	// Use this for initialization
	void Start () {
		
		blueScoreText = GameObject.Find ("BlueScore").GetComponent<Text>();
		redScoreText = GameObject.Find ("RedScore").GetComponent<Text>();
		blueScoreText.text = "0";
		redScoreText.text = "0";
		
		
		
		NetworkServer.RegisterHandler (chatMsg, OnServerUpdateScore);
	}
	
	// Update is called once per frame
	void Update () {
		blueScoreText.text = "Blue: " + blueScore.ToString ();
		redScoreText.text = "Red: " + redScore.ToString ();
	}
	
	[Server]
	void OnServerUpdateScore(NetworkMessage netMsg){
		string team = netMsg.ReadMessage<StringMessage>().value;
		if (team == "blue") {
			blueScore += 1;
		} else {
			redScore += 1;
		}
	}
	
	[Client]
	public void UpdateScore(string team){
		var msg = new StringMessage (team);
		NetworkManager.singleton.client.Send (chatMsg, msg);
	}
	
	/*
	public void UpdateScore(string team){
		if (team == "blue") {
			blueScore += 1;
			CmdUpdateScore(blueScore, "blue");
		} else {
			redScore += 1;
			CmdUpdateScore(redScore, "red");
		}
	}

	[Command]
	void CmdUpdateScore(int score, string team){
		if (team == "blue") {
			blueScore = score;
		} else {
			redScore = score;
		}
	} */
	
}
