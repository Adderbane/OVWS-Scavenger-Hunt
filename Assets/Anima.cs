using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public enum AnimState
{
	Jump,
	Walk,
	Run,
	Stay
}

public class Anima : NetworkBehaviour {
	
	Actions actions;

	[SyncVar (hook="StateSwap")]
	AnimState state;

	// Use this for initialization
	void Start () {
		actions = GetComponentInChildren<Actions> ();
		state = AnimState.Stay;
	}
	
	// Update is called once per frame
	void Update () {
		if (isLocalPlayer) {
			if (Input.GetKeyDown (KeyCode.Space)) {
				CmdSetState(AnimState.Jump);
			}
			else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D)) {
				if (Input.GetKey(KeyCode.LeftShift)) {
					CmdSetState(AnimState.Run);
				}
				else CmdSetState(AnimState.Walk);
			}
			else if (Input.GetKeyDown(KeyCode.LeftShift)) {
				if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) {
					CmdSetState(AnimState.Run);
				}
			}
			else if (Input.GetKeyUp (KeyCode.LeftShift)) {
				if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) {
					CmdSetState(AnimState.Walk);
				}
			}
			else if (!(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))) {
				CmdSetState(AnimState.Stay);
			}
		}
	}

	[Command]
	void CmdSetState(AnimState newState)
	{
		state = newState;
	}

	[Client]
	void StateSwap(AnimState newState)
	{
		switch (newState) {
		case AnimState.Jump:
			actions.Jump();
			break;
		case AnimState.Run:
			actions.Run();
			break;
		case AnimState.Stay:
			actions.Stay();
			break;
		case AnimState.Walk:
			actions.Walk();
			break;		
		default:
			break;
		}
	}
}
