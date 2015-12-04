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

	void Start ()
	{
		print ("MyNetworkManager : Start");
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
		SetIPAddress ();
		SetPort ();
		team = "red";
		username = GameObject.Find ("txtUsername").transform.FindChild ("Text").GetComponent<Text> ().text;
		NetworkManager.singleton.StartHost ();
	}

	//Join Red button
	public void JoinR ()
	{
		SetIPAddress ();
		SetPort ();
		username = GameObject.Find ("txtUsername").transform.FindChild ("Text").GetComponent<Text> ().text;
		team = "red";	
		NetworkManager.singleton.StartClient ();
	}
	//Join Blue button
	public void JoinB ()
	{
		SetIPAddress ();
		SetPort ();
		username = GameObject.Find ("txtUsername").transform.FindChild ("Text").GetComponent<Text> ().text;
		team = "blue";
		NetworkManager.singleton.StartClient ();
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
		//BackgroundSoundChange sound = GameObject.Find ("BackgroundSound").GetComponent<BackgroundSoundChange> ();
		//StartCoroutine(sound.MusicTimer());
	}
}
