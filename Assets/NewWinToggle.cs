using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NewWinToggle : MonoBehaviour {

	//public Image win;

	// Use this for initialization
	void Start () {
		gameObject.SetActive (true);
		//maybe separate script that points to object
		//event code to activate it on/off in a different script
	}
	
	// Update is called once per frame
	void Update () {
	if(Input.GetKeyDown(KeyCode.C)) {
			gameObject.SetActive(false);
			//win.enabled = false;
		}
	}
	
}
