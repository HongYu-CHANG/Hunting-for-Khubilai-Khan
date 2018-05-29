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

		if (collision.gameObject.name != "StartAnimal" && collision.gameObject.name != "Horse")
		{
			Destroy(this.gameObject);
		}
		if (collision.gameObject.name == "Card1" || collision.gameObject.name == "Card2" || collision.gameObject.name == "Card3")
		{
			Destroy(this.gameObject);
		}
	}
}
