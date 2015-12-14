using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Minimap : MonoBehaviour {

	public Sprite redDot;
	public Sprite blueDot;
	Sprite playerDot;
	GameObject player;
	GameObject dot;
	string team;
	float pX;
	float pY;
	float dX;
	float dY;

	// Use this for initialization
	void Start () {
		team = GameObject.Find ("NetManager").GetComponent<MyNetworkManager> ().team;
		if (team == "red") {
			playerDot = redDot;
		}
		else {
			playerDot = blueDot;
		}
	}
	
	// Update is called once per frame
	void Update () {

		if (player == null) {
			GameObject g = GameObject.Find ("NetManager");
			MyNetworkManager m = g.GetComponent<MyNetworkManager> ();
			player = m.client.connection.playerControllers[0].gameObject;
		}
		if (dot == null) {
			dot = GameObject.FindGameObjectWithTag ("MapDot");
			//dot.GetComponent<Image>().sprite = playerDot;
		}
		//Get player coordinates
		pX = player.transform.position.x;
		pY = player.transform.position.z;

		//Map to map coordinates X(1250 -- -250) -> X(-100 -- 100)   Z(1250 -- -250) -> Y(-100 -- 100)
		dX = (((pX - 500.0f) * (-1.0f)) / (750.0f)) * 100.0f;
		dY = (((pY - 500.0f) * (-1.0f)) / (750.0f)) * 100.0f;

		//Move dot
		dot.GetComponent<RectTransform>().anchoredPosition = new Vector2(dX, dY);
	}
}
