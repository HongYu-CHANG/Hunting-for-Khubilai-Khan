using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMovement : MonoBehaviour {
	private int frameSegment = 1;
	private int counter = 0;
	private float deltaPos = 0.05f;
	private int adding = 0;
	private float deltaRot = 100f;
	private bool isShot = false;
	private Vector3 positionInfo;
	private Vector3 rotationInfo;

	// Use this for initialization
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		positionInfo = this.transform.position;
		rotationInfo = this.transform.eulerAngles;
		if (counter == frameSegment)
		{
			adding++;
			if (adding == 15)
			{
				adding = 0;
				deltaPos = -deltaPos;
			}
			this.transform.position = new Vector3(positionInfo.x, positionInfo.y + UnityEngine.Random.Range(0f, deltaPos) * Time.deltaTime, positionInfo.z);
			if (isShot && (rotationInfo.y > 270 || rotationInfo.y < 91))
			{
				transform.eulerAngles = new Vector3(0, rotationInfo.y - UnityEngine.Random.Range(0f, deltaRot) * Time.deltaTime, 0);
			}
			else
			{
				isShot = false;
			}
		}
		else
		{
			counter++;
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("arrow") && collision.gameObject.transform.parent == GameObject.FindWithTag("horse").transform)
		{
			isShot = true;
		}
	}
}
