using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// Put this script on any gameobject
// An empty one should do
public class ToggleWinScreen : MonoBehaviour {

	public Canvas winCanvas; //drag and drop canvas in the editor in the script component
	private bool isActive;


	// Use this for initialization
	void Start () {
		isActive = false;
	}
	
	// Update is called once per frame
	// Enable with key press for now
	// Later move it to other script, enabled by score
	void Update () {
	if (Input.GetKeyDown (KeyCode.X)) {
			isActive = true;
			//winCanvas.SetActive(isActive);
			winCanvas.gameObject.SetActive(isActive);
			Debug.Log(isActive);
		}
	}
}
