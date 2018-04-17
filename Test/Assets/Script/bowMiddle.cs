using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bowMiddle : MonoBehaviour {

	public bool collideStatus;
	void Start () {
		collideStatus = false;
	}
	public bool returnStatus(){
		return collideStatus;
	}
	void OnCollisionEnter(){
		collideStatus = true;
	}

	void OnCollisionExit(){
		collideStatus = false;
	}
}
