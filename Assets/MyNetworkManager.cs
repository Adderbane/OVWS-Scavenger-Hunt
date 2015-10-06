using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MyNetworkManager: NetworkManager
{

	public static NetworkClient myClient;

	[SerializeField]
	Button
		hostButton;
	[SerializeField]
	Button
		joinButton;
	[SerializeField]
	Button
		joinRed;
	[SerializeField]
	Button
		joinBlue;
	[SerializeField]
	InputField
		usernameText;
	[SerializeField]
	InputField
		IPButton;
	
	Chat chat;
	public string username;
	public string team;

	//holds usernames, because you can not dynamically push to an array in Unity
	List<string> playerNames = new List<string>();
	//
	private Vector3[] spawnPointsRed; //fill up with predefined spawn points
	private Vector3[] spawnPointsBlue; //fill up with predefined spawn points
	//
	private string[] playersTeams;
	//List<Vector3> playersTeams; 

	void Start ()
	{
		print ("MyNetworkManager : Start");
		//
		spawnPointsRed = new Vector3[3];
		spawnPointsBlue = new Vector3[3];

		spawnPointsRed [0] = new Vector3 (62.8f, 1f, 184.2f);
		spawnPointsRed [1] = new Vector3 (-18.6f, 1f, 184.2f);
		spawnPointsRed [2] = new Vector3 (-77.2f, 1f, 184.2f);
		//
		spawnPointsBlue [0] = new Vector3 (-100.8f, 1f, 34.7f);
		spawnPointsBlue [1] = new Vector3 (-61.6f, 1f, 34.7f);
		spawnPointsBlue [2] = new Vector3 (-22f, 1f, 34.7f);

	}
	

	//get input from text input fields
	void SetIPAddress ()
	{
		string ipAddress = GameObject.Find ("txtIP").transform.FindChild ("Text").GetComponent<Text> ().text;
		if (ipAddress == "") {
			ipAddress = "127.0.0.1";
		}
		NetworkManager.singleton.networkAddress = ipAddress;
	}
	
	void SetPort ()
	{
		NetworkManager.singleton.networkPort = 7777;
	}
	
	// button event handlers
	public void StartupHost ()
	{
		print ("clicked button, starting host (we hope)");
		SetIPAddress ();
		SetPort ();
		team = "red";
		username = GameObject.Find ("txtUsername").transform.FindChild ("Text").GetComponent<Text> ().text;
		NetworkManager.singleton.StartHost ();
	}
	
	public void JoinGame ()
	{
		print ("clicked button, join game");
		SetIPAddress ();
		SetPort ();
		team = "blue";
		username = GameObject.Find ("txtUsername").transform.FindChild ("Text").GetComponent<Text> ().text;

		playerNames.Add (username);

		NetworkManager.singleton.StartClient ();
	}
	//Join Red button
	public void JoinR ()
	{
		print ("clicked button, join game");
		SetIPAddress ();
		SetPort ();
		username = GameObject.Find ("txtUsername").transform.FindChild ("Text").GetComponent<Text> ().text;
		
		playerNames.Add (username); //store username
		playersTeams [0] = "Red"; //assign to team
		
		NetworkManager.singleton.StartClient ();
	}
	//Join Blue button
	public void JoinB ()
	{
		print ("clicked button, join game");
		SetIPAddress ();
		SetPort ();
		username = GameObject.Find ("txtUsername").transform.FindChild ("Text").GetComponent<Text> ().text;
		
		playerNames.Add (username); //store username
		playersTeams [1] = "Blue"; //assign to team (not perfect)
		
		NetworkManager.singleton.StartClient ();
	}
	//
	//overrides the way player objects are created
	//we need this for spawn points
	public virtual void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		if (playersTeams [0] == "Red") {
		    //give new material
			print ("redteam");
			//playerPrefab.GetComponentInChildren<MeshRenderer>().material = newRed;
			//spawn on Red side (test point)
			var player = (GameObject)GameObject.Instantiate (playerPrefab, spawnPointsRed [0], Quaternion.identity);
			NetworkServer.AddPlayerForConnection (conn, player, playerControllerId);
		}
		if (playersTeams [1] == "Blue") {
			//give new material
			//playerPrefab.GetComponentInChildren<MeshRenderer>().material = newBlue;
			//spawn on Blue side (test point)
			var player = (GameObject)GameObject.Instantiate (playerPrefab, spawnPointsBlue [0], Quaternion.identity);
			NetworkServer.AddPlayerForConnection (conn, player, playerControllerId);
		}
	}
	
	public void Disconnect ()
	{
		NetworkManager.singleton.StopClient ();
	}
	
	//make sure correct buttons appear on different scenes 
	void OnLevelWasLoaded (int level)
	{
		print ("Level Loaded : " + level);
		if (level == 0) {
			SetupLoginButtons ();
		} else {
			SetupChatSceneButtons ();
		}
	}
	
	
	//Do this in code because when we leave a scene, all objects are destroyed. 
	//Then when we return, the objects are new, and we will have lost our references.
	void SetupLoginButtons ()
	{
		print ("MyNetworkManager : SetupMenuSceneButtons");
		
		GameObject.Find ("btnHostGame").GetComponent<Button> ().onClick.RemoveAllListeners ();
		GameObject.Find ("btnHostGame").GetComponent<Button> ().onClick.AddListener (StartupHost);
		
		GameObject.Find ("btnJoinGame").GetComponent<Button> ().onClick.RemoveAllListeners ();
		GameObject.Find ("btnJoinGame").GetComponent<Button> ().onClick.AddListener (JoinGame);
		//
		GameObject.Find ("btnJoinRed").GetComponent<Button> ().onClick.RemoveAllListeners ();
		GameObject.Find ("btnJoinRed").GetComponent<Button> ().onClick.AddListener (JoinR);
		//
		GameObject.Find ("btnJoinBlue").GetComponent<Button> ().onClick.RemoveAllListeners ();
		GameObject.Find ("btnJoinBlue").GetComponent<Button> ().onClick.AddListener (JoinB);
		
	}
	
	void SetupChatSceneButtons ()
	{
		GameObject.Find ("btnDisconnect").GetComponent<Button> ().onClick.RemoveAllListeners ();
		GameObject.Find ("btnDisconnect").GetComponent<Button> ().onClick.AddListener (NetworkManager.singleton.StopClient);
	}
}
