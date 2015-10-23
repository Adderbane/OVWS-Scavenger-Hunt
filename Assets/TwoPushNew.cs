using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TwoPushNew : ChaseTarget {


	//Number of players in contact with an object

	int numRed;

	int numBlue;
	
	//public GameObject rollinRock;
	//private Rigidbody rb;
	
	//private Transform coolBeans;
	
	//public Vector3 velocity;
	public Transform target;
	public float speed;
	
	bool isRed;
	bool isBlue;
	
	// Use this for initialization
	void Start () {
		numRed = 0;
		numBlue = 0;
		//rb = GetComponent<Rigidbody>();
		//coolBeans = transform.position.x;
		isRed = false;
		isBlue = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (isServer) {
			if (numRed >= 2) {
				isRed = true;
				CmdCaught("red");
			}
			else if (numBlue >= 2) {
				isBlue = true;
				CmdCaught("blue");
			}
		}
	}
	
	//Recognize people hitting an object
	protected override void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.attachedRigidbody.gameObject.tag == "Player") {
			string team = collision.collider.attachedRigidbody.gameObject.GetComponent<PlayerIdentity>().myTeam;
			Enter(team);
		}
	}
	
	//People leave an object
	void OnCollisionExit(Collision collision)
	{
		if (collision.collider.attachedRigidbody.gameObject.tag == "Player") {
			string team = collision.collider.attachedRigidbody.gameObject.GetComponent<PlayerIdentity>().myTeam;
			Leave(team);
		}
	}
	
	//Called from CollisionEnter
	void Enter(string team)
	{
		if (team == "red") {
			numRed++;
			Debug.Log(numRed);
		}
		else {
			numBlue++;
		}
	}
	
	//Called from collision leave
	void Leave(string team)
	{
		if (team == "red") {
			numRed--;
			Debug.Log(numRed);
		}
		else {
			numBlue--;
		}
	}
	
	//Called on server when collision criteria are met
	protected override void CmdCaught(string newWin)
	{
		//numRed = 0;
		//numBlue = 0;
		//
		//Vector3 myForward = transform.TransformDirection (Vector3.forward);
		//rb.AddForce (myForward * 100.0f);
		
		//coolBeans += 20.0f;
		
		//transform.Translate (velocity * Time.deltaTime);
		//this.transform.position += Vector3.up;
		
		//transform.Translate (0, Time.deltaTime, 0, Space.Self);
		if (isRed == true) {
			/*float step = speed * Time.deltaTime;
			transform.position = Vector3.MoveTowards (transform.position, target.position, step);
			Debug.Log ("This thing should be pushed");*/
			
			transform.Translate(Vector3.forward * Time.deltaTime);
		}
	}

}
