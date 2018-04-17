using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class horseControl : MonoBehaviour {

	public SteamVR_TrackedObject playerHead;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.H))
		{
			Debug.Log("Press H");
			transform.position = playerHead.transform.position;
			//we want to move horse backward, how?
			//transform.position = new Vector3(playerHead.transform.position.x, playerHead.transform.position.y, playerHead.transform.position.z);
			Debug.Log("position: " + transform.position);
			transform.forward = playerHead.transform.forward;
			transform.position -= transform.up.normalized * 1.5f;
		}

	}

}
