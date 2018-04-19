using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedEnvironmentController : MonoBehaviour {

    // transform that we will follow
    public Transform follow;
	
	// Update is called once per frame
	void Update ()
	{
        transform.position -= follow.transform.forward.normalized * 5 * Time.deltaTime;
	}
}
