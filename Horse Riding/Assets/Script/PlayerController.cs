using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    //running speed
    public float runningSpeed = 5;

    //jumping force
    public float jumpForce = 5;

    //threshold speed y
    public float jumpSpeedLimit;

    //time step for speed calculation (seconds)
    public float timeStep;

    //elapsed time
    public float elapsedTime = 0;

    //rigid body component
    Rigidbody rb;

    //collider component
    Collider col;

    //direction of movement
    Vector3 direction;

    //keep track of the previous position
    float previousY;   

	// Use this for initialization
	void Start () {
        //get our rigid body
        rb = GetComponent<Rigidbody>();

        //get our collider
        col = GetComponent<Collider>();

        //init our direction
        direction = Vector3.forward;

        //init previousY
        previousY = Camera.main.transform.position.y;
    }

    // Update is called once per frame
    void FixedUpdate () {
                
        // set running speed
        rb.velocity = new Vector3(direction.x * runningSpeed,rb.velocity.y , direction.z * runningSpeed) ;

        //listen user input
        if(Input.GetButtonDown("Fire1"))
        {
            //make the player jump
            Jump();
        }

        //increase elapsed time
        elapsedTime += Time.fixedDeltaTime;

        if(elapsedTime >= timeStep)
        {
            // distance: compare current position y of the camera with the previous
            float diffY = Camera.main.transform.position.y - previousY;

            // speed = distance / time
            float speedY = diffY / elapsedTime;

            // reset elapsed time
            elapsedTime = 0;

            // compare with speed threshold
            // check that we are grounded
            if (speedY > jumpSpeedLimit && CheckGrounded())
            {
                // jump
                Jump();
            }

            // update our "previous" value
            previousY = Camera.main.transform.position.y;
        }

        

    }

    void Jump()
    {
        //check that we are grounded
        if(CheckGrounded())
        {
            //add a force to jump
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }        
    }

    bool CheckGrounded()
    {
        //get the height of the player
        float sizeY = col.bounds.size.y;

        //return whether we are grounded or not
        return Physics.Raycast(transform.position, Vector3.down, sizeY / 2);
    }

    void OnCollisionEnter(Collision collision)
    {
        // check if we run into water
        if(collision.collider.CompareTag("Water"))
        {
            print("you've fallen!");
            SceneManager.LoadScene("Game");
        }
    }

}
