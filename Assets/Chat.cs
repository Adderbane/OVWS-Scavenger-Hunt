using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class Chat : NetworkBehaviour
{
	const short chatMsg = MsgType.Highest + 1;

	private SyncListString chatLog = new SyncListString ();

	public static NetworkClient myClient;

	[SerializeField]
	InputField
		chatInput;
	[SerializeField]
	Text
		chatWindow;
	string username;
	string team;
	


//	 Use this for initialization
	void Start ()
	{

		chatLog.Callback = OnChatUpdated;

		//setup text boxes
		chatWindow.text = "";
		username = GameObject.Find ("NetManager").GetComponent<MyNetworkManager> ().username;
		team = GameObject.Find ("NetManager").GetComponent<MyNetworkManager> ().team;

		NetworkServer.RegisterHandler (chatMsg, OnServerPostChatMessage);   

		chatInput.onEndEdit.AddListener (delegate {
			PostChatMessage (chatInput.text);
		}); 
	}

	public override void OnStartClient ()
	{
		//Callback is the delegate type used for SyncListChanged.
		chatLog.Callback = OnChatUpdated;
	}

	/*
	 * [Server] 
	 * A Custom Attribute that can be added to member functions of NetworkBehaviour scripts, 
	 * to make them only run on servers.
	 * 
	 * A [Server] method returns immediately if NetworkServer.active is not true, 
	 * and generates a warning on the console. This attribute can be put on member 
	 * functions that are meant to be only called on server. This would redundant for 
	 * [Command] functions, as being server-only is already enforced for them.
	 */
	[Server]
	void OnServerPostChatMessage (NetworkMessage netMsg)
	{
		string message = netMsg.ReadMessage<StringMessage> ().value;
		chatLog.Add (message);
	}
	/*
	 * [Client]
	 * makes a NetworkBehaviour script only run on clients.
	 * 
	 * A [Client] method returns immediately if NetworkClient.active is not true, 
	 * and generates a warning on the console. This attribute can be put on member 
	 * functions that are meant to be only called on clients. This would redundant 
	 * for [ClientRPC] functions, as being client-only is already enforced for them.
	 */
	[Client]
	public void PostChatMessage (string message)
	{
		if (message.Length == 0)
			return;

		//Creates a teambit based on team
		string teamBit = team.Substring (0, 1);

		//Checks if the message begins "/all " and changes teambit to a if so
		if (message.Length > 5) {
			if (message.Substring (0, 5) == "/all ") {
				teamBit = "a";

				//Slices /all off the message
				message = message.Remove (0, 5);
			}
		}

		//Compiles message with teambit at start
		var msg = new StringMessage (teamBit + username + ": " + message);
		NetworkManager.singleton.client.Send (chatMsg, msg);
		
		chatInput.text = "";
		chatInput.Select ();
		chatInput.ActivateInputField ();
	}

	//callback we registered for when the syncList changes
	private void OnChatUpdated (SyncListString.Operation op, int index)
	{
		//Checks the teambit and only posts if appropriate
		string newMessage = chatLog [chatLog.Count - 1] + "\n";
		if (newMessage.Substring(0,1) == "a") {
			newMessage = newMessage.Remove(0,1);
			newMessage = "[All] " + newMessage;
			chatWindow.text += newMessage;
		}
		else if (team.Substring(0,1) == newMessage.Substring(0,1)) {
			newMessage = newMessage.Remove(0,1);
			newMessage = "[Team] " + newMessage;
			chatWindow.text += newMessage;
		}
	}



}
