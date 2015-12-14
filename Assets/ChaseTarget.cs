using UnityEngine;
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
			if(Input.GetKeyDown(KeyCode.R)){
				string team = colliderId.myTeam;
				Caught(team, score);
				colliderId.PopupOn(popupText);
				colliderId.SetLastClue(popupText);
			}
		}
	}

	[Client]
	void Move(Vector3 newPos)
	{
		gameObject.transform.position = newPos;
	}

	//Call Caught command when player collides
    protected virtual void OnTriggerEnter(Collider collider)
	{
		if (collider.GetComponentInParent<Transform>().tag == "Player") {
			isTouch = true;
			colliderId = collider.GetComponentInParent<PlayerIdentity>();

			//Let's test this badboy out
			/*Application.LoadLevel (Application.loadedLevel);
			Debug.Log("Reloaded");*/
		}
	}
	
	protected virtual void OnTriggerExit(Collider collider){
		if (collider.GetComponentInParent<Transform>().tag == "Player") {
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
