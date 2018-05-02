using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.name != "Horse" 
			&& gameObject.transform.parent == GameObject.FindWithTag("horse").transform)
		{
			Destroy(this.gameObject);
			SendMessageUpwards("AddScore", collision.gameObject.name);
		}
	}
}
