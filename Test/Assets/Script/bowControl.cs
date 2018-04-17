using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bowControl : MonoBehaviour {
    //--------------VR Relating--------------//
    public SteamVR_TrackedController playerPullHand;
    public SteamVR_TrackedController playerBowHand;
    //--------------VR Relating--------------//

	//-------------Line Renderer-------------//  
    private LineRenderer lineRenderer;   
    private int numberOfPointsOnBow = 3;
    //-------------Line Renderer-------------//

    //-------------Bow Initiaters------------//
    private GameObject bowTop;
	private GameObject bowBot;
	private bowMiddle bowMiddle;
    private GameObject bowBody;

    // Rigidbody of bowMiddle
    private Rigidbody handBody;
    //-------------Bow Initiaters------------//

    //-------------Bow Settings--------------//
    public float pullBackCoefficient;
    public float arrowShootCoefficient;
    //-------------Bow Settings--------------//

    //-------------Bow Controls--------------//

    // Calculate the accumulated time for exponential decay function
    public int dampingTime = 8;

    // Used to count the time of damping
    private int dampingCount;

    // Indicates whether damping is still needed
    private bool damping;

    // Displacement of the bowstring
    private Vector3 displacement;

    // Record the middle point where the player pulls
    private Vector3 bowMiddleOriginal;

    // Vector3 array for linerenderer
    private Vector3[] bowPositions;
	//-------------Bow Controls--------------//

    //------------Arrow Controls-------------//
    public Rigidbody arrow;
    private Rigidbody arrowClone;

    // Indicates whether arrow exists
    private bool arrowExist;
    private bool shotMade;
    private bool isNear;
    
    //------------Arrow Controls-------------//

    void Start()  
    {   
    //-------------Get objects---------------//
        bowMiddle = gameObject.GetComponentInChildren<bowMiddle>();
        bowTop = transform.Find("bowTop").gameObject;
		bowBot = transform.Find("bowBot").gameObject;
        bowBody = transform.Find("bowBody").gameObject;
        handBody = bowMiddle.GetComponent<Rigidbody>();
    //-------------Get objects---------------//

    //-------------Set objects---------------//
        bowMiddle.transform.position = bowTop.transform.position + getMiddleFrom1To2(bowTop, bowBot);
        handBody.freezeRotation = true;
        //-------------Set objects---------------//

        //----------Set up linerender------------//
        lineRenderer = gameObject.AddComponent<LineRenderer>();  

        // Set material 
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));  

        // Set color  
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.white;

        // Set width  
        lineRenderer.startWidth = 0.01f;  
        lineRenderer.endWidth = 0.01f;

        // Set the number of points on the bow string
        lineRenderer.positionCount = numberOfPointsOnBow;
    //----------Set up linerender------------//

    //----------Bowstring control------------//
        dampingCount = 0;
        damping = false;
        bowMiddleOriginal = bowTop.transform.position + getMiddleFrom1To2(bowTop, bowBot);
        bowPositions = new Vector3[numberOfPointsOnBow];
        displacement = new Vector3(0, 0, 0);
	//----------Bowstring control------------//

    //------------Arrow Controls-------------//
        arrowExist = false;
        shotMade = false;
        isNear = false;
    //------------Arrow Controls-------------//
    }  
  
    void Update()  
    {   

        // Update the middle point of bow
        bowMiddleOriginal = bowTop.transform.position + getMiddleFrom1To2(bowTop, bowBot);
        
        // If there's arrow on the bow, update its position and rotation
        if(arrowExist){

            // Update the rotation of the arrow
            arrowClone.transform.up = handBody.transform.forward;

            // Update the position of the arrow
            arrowClone.transform.position = playerPullHand.transform.position + handBody.transform.forward.normalized * arrowClone.transform.localScale.y;
        }
        
        // Get the linerenderer  
        lineRenderer = GetComponent<LineRenderer>();  
       
        // Control of the bow
        if(playerPullHand.triggerPressed && isNear){
            if(!arrowExist){

                // Generates the arrow                
                arrowClone = Instantiate(arrow, bowMiddle.transform.position + getMiddleFrom1To2(bowMiddle.gameObject, bowBody), bowBody.transform.rotation);
                
                // Move the arrow to the tip of player's hand
                arrowClone.transform.position += bowBody.transform.forward.normalized * arrowClone.transform.localScale.y;

                // Rotate the arrow to face the shooting direction
                arrowClone.transform.up = -1*bowBody.transform.forward;

                // Temporarily disable collision
                arrowClone.detectCollisions = false;

                // Set the shoot state
                arrowExist = true;
                shotMade = false;

            }

            // Displacement is the vector between pull hand and the bow
            displacement = handBody.transform.position - bowMiddleOriginal;

            // Update position of handbody to enable the pull
            handBody.transform.position = playerPullHand.transform.position;

            // Once pulled, set the status to true to indicate the player is pulling the bow and damping is needed
            damping = true;
        }

        // Player let go of the bowstring
        else{

            isNear = false;
            // If the arrow exists
            if(arrowExist){

                // If the arrow is still on the bow
                if(!shotMade){

                    // Enable arrow's collider
                    arrowClone.detectCollisions = true;

                    // Enable arrow's gravity
                    arrowClone.useGravity = true;

                    // Apply force to the arrow
                    arrowClone.AddForce(scaleVector3(-bowBody.transform.forward, -displacement.magnitude*arrowShootCoefficient));
                    // Reset shoot status
                    shotMade = true;
                    arrowExist = false;
                }
            }

            // If haven't reach the limit on the time of damping, and the player has pulled the bow
            if((dampingCount < dampingTime) && damping){

                // Bounce back the bowstring. 
                handBody.AddForce(scaleVector3((bowBody.transform.position - handBody.transform.position).normalized, pullBackCoefficient));

                // If bowstring reached the bow
                if(bowMiddle.returnStatus()){
                    dampingCount++;
                }
            }

            // Damping effect done, reset parameters
            else{
                handBody.transform.position = bowMiddleOriginal;
                displacement -= displacement;              
                dampingCount = 0;
                damping = false;
            }
        }

        // Set the parameters for linerenderer
        bowPositions[0] = bowTop.transform.position;
        bowPositions[1] = bowMiddle.transform.position;
        bowPositions[2] = bowBot.transform.position;

        // Render the bowstring
        lineRenderer.SetPositions(bowPositions);
        
    }  

    // Used to scale Vector3 by scaler
    Vector3 scaleVector3(Vector3 vec, float scaler){
        vec.x *= scaler;
        vec.y *= scaler;
        vec.z *= scaler;

        return vec;
    }

    // Get the vector to the middle point between object 1 and object 2 in the direction of from 1 to 2
    Vector3 getMiddleFrom1To2(GameObject object_1, GameObject object_2){
        Vector3 returnVec;
        returnVec.x = (object_2.transform.position.x - object_1.transform.position.x)/2;
        returnVec.y = (object_2.transform.position.y - object_1.transform.position.y)/2;
        returnVec.z = (object_2.transform.position.z - object_1.transform.position.z)/2;
        return returnVec;
    }

    void OnTriggerStay(Collider other) 
    {
       isNear = true;
    }
}

