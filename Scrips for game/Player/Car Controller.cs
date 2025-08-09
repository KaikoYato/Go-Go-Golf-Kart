using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private Rigidbody theRB;
    private CameraSwitch camSwitch;
    private bool ReverseCamActive = false;
   [SerializeField] private float forwardAccel = 8f, reverseAccel = 4f, maxSpeed = 50f, 
        turnStrength = 180, gravityForce = 10f, dragOnGround = 0.02f;
    private float speedInput, turnInput;
    [SerializeField] private float currentSpeed;
    [SerializeField] private float accelerationRate = 1.2f;
    [SerializeField] private float decelerationRate = 0.01f;
    [SerializeField] private bool grounded;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float groundRayLength = 0.5f;
    [SerializeField] private Transform groundRayPoint;

    [SerializeField] private Transform leftFrontWheel, rightFrontWheel;
    [SerializeField] private float maxWheelTurn = 25f;
    
    public bool canMove = false;
   
    void Start()
    {
        theRB.transform.parent = null; //to prevent jittering movement of the car model
         camSwitch = FindObjectOfType<CameraSwitch>();
    }
    private void Update()
    { if (Input.GetAxis("Vertical") < 0 && !ReverseCamActive )
        {
            Debug.Log("reverse cam active");
            camSwitch.switchCam(camSwitch.cam2);
            ReverseCamActive = true;
        }
        else if (Input.GetAxis("Vertical") > 0 && ReverseCamActive)
        {
            Debug.Log("Normal cam active");
            camSwitch.switchCam(camSwitch.cam1);
            ReverseCamActive = false;
        }
        
        leftFrontWheel.localRotation = Quaternion.Euler(leftFrontWheel.localRotation.eulerAngles.x, 
            turnInput * maxWheelTurn, leftFrontWheel.localRotation.eulerAngles.z);
        rightFrontWheel.localRotation = Quaternion.Euler(rightFrontWheel.localRotation.eulerAngles.x, 
            turnInput * maxWheelTurn, rightFrontWheel.localRotation.eulerAngles.z);

    }
    private void FixedUpdate()
    { if (canMove){
        grounded = false;
        RaycastHit hit;
        if (Physics.Raycast(groundRayPoint.position, -transform.up,
                out hit, groundRayLength, whatIsGround)) //Checks if the raycast is hitting the ground
        {
            grounded = true;
            Console.WriteLine("You're Hitting the ground OG");
            //This is to make it so the car isn't flat and actually tilts to interact with the terian and surface it is on. For ramps it will tilt upwards etc etc
            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation; 
        }
        speedInput = 0f;
        if (Mathf.Abs(Input.GetAxis("Vertical")) > 0) 
        {
            float acceleration = Input.GetAxis("Vertical") > 0 ? forwardAccel : reverseAccel; // this is a condensend if statement, technically saying if the input axis is greater than 0 its  will use foward accle other than that it will use reverse accel
            speedInput = Input.GetAxis("Vertical") * acceleration * 10f;
        }
        else // if theres no input, the cars movment will lerp and slow down
        {
            speedInput = Mathf.Lerp(speedInput, 0, decelerationRate * 0.1f * Time.deltaTime);
            theRB.linearDamping = 0.05f;
        }

// using lerp for smoother acceleration
        currentSpeed = Mathf.Lerp(currentSpeed, speedInput, accelerationRate * Time.deltaTime);
        turnInput = Input.GetAxis("Horizontal");
        if (grounded) //only accepts input if on the ground
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput *
                turnStrength * Time.deltaTime ,
                0f)); //Rather than add a force left and right, we rotate the object left andright and continue it's forwards of backwards motion
        }

        transform.position = theRB.transform.position;
        if (grounded)//only accepts input if on the ground
        {
            theRB.linearDamping = dragOnGround;
            theRB.velocity = Vector3.Lerp(theRB.velocity, transform.forward * currentSpeed, accelerationRate * Time.deltaTime);
        }
        else if (!grounded)
        {
            theRB.linearDamping = 0.01f;
            float pushdown = -gravityForce * 90f;
            theRB.AddForce(Vector3.up * pushdown); //pushes it down when the car is not on the ground.
        }

    }
        }
}