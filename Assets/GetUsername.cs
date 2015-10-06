using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class GetUsername : NetworkBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<TextMesh> ().text = GetComponentInParent<PlayerIdentity> ().myUsername;
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = Camera.main.transform.rotation;
	}
}
