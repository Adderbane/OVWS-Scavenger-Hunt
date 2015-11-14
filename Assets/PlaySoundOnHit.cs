using UnityEngine;
using System.Collections;

public class PlaySoundOnHit : MonoBehaviour {
	
	bool fired;

	// Use this for initialization
	void Start () {
		fired = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		if (fired == false) {
			GetComponent<AudioSource>().Play();
			fired = true;
		}
	}
}
