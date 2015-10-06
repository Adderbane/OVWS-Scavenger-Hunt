using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class GetUsername : NetworkBehaviour {

	TextMesh t;
	// Use this for initialization
	void Start () {
		t = GetComponent<TextMesh> ();
		t.text = GetComponentInParent<PlayerIdentity> ().myUsername;
	}
	
	// Update is called once per frame
	void Update () {
		if (t.text == "") {
			t.text = GetComponentInParent<PlayerIdentity> ().myUsername;
		}
		transform.rotation = Camera.main.transform.rotation;
	}
}
