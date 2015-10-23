using UnityEngine;
using System.Collections;

public class Bouncy : MonoBehaviour {
	float weight;


	// Use this for initialization
	void Start () {
		weight = 20.0f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnControllerColliderHit(ControllerColliderHit hit) {
		if (hit.rigidbody) {
			hit.rigidbody.AddForceAtPosition(-Vector3.up * weight, hit.point);
		}
	}
}
