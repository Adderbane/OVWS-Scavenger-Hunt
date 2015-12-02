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
		int score = int.Parse(team.Substring (team.Length - 1));
		team = team.Substring (0, team.Length - 1);
		if (team == "blue" && score > blueScore) {
			blueScore = score;
		} else if (team == "red" && score > redScore) {
			redScore = score;
		}

		//let's test this out
		if (blueScore == 2 || redScore == 2) {
			//this line of code is from "ForcedReset.cs"
			// check the scripts
			//it should work
			Application.LoadLevelAsync(Application.loadedLevelName);
		}
	}
	//when score reaches a certain level, reset scene or
	//disconnect all players
	//disconnect would not work, it does not reset state of world
	
	[Client]
	public void UpdateScore(string team, string score){
		//all this does is send a message across clients
		//so that score can be viewed
		var msg = new StringMessage (team+score);
		NetworkManager.singleton.client.Send (chatMsg, msg);
	}

}
