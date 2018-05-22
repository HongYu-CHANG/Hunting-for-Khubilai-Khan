using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMovement : MonoBehaviour {
	private int frameSegment = 1;
	private int counter = 0;
	private float deltaPos = 0.05f;
	private int adding = 0;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (counter == frameSegment)
		{
			adding++;
			if (adding == 15)
			{
				adding = 0;
				deltaPos = -deltaPos;
			}
			this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + UnityEngine.Random.Range(0f, deltaPos) * Time.deltaTime, this.transform.position.z); //this.transform.position.x + deltaPos;
			counter = 0;
		}
		else
		{
			counter++;
		}
	}
}
