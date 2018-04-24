using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedEnvironmentController : MonoBehaviour {

  // transform that we will follow
  public Transform follow;
	private int speed;

	// Update is called once per frame
	void Update ()
	{
		transform.position -= follow.transform.forward.normalized * this.speed * Time.deltaTime;
	}
	public void setSpeed(int speed)
	{
		this.speed = speed;
	}
}
