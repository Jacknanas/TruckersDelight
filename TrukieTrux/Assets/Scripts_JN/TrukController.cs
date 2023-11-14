using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrukController : MonoBehaviour
{
    private Vector3 velocity;
    public int gear = 1;
    public float speed = 0;
    private Rigidbody rb;
    private float movementX;
    private float movementZ = 2.0f;
    private float timeSinceStart;
    private bool stall = false;
    
    public float maxSpeed = 10f;

    public float direction = 90;
    
    public GearChangingUI gearChangeUI;
    public float torque = 1;


    public Vector3 RotateVectorAroundY(float angleDegrees, Vector3 originalVector)
    {
        // Convert angle to radians
        float angleRadians = Mathf.Deg2Rad * angleDegrees;

        // Create a quaternion for the rotation around the Y axis
        Quaternion rotationQuaternion = Quaternion.Euler(0, angleDegrees, 0);

        // Apply the rotation to the original vector
        Vector3 rotatedVector = rotationQuaternion * originalVector;

        return rotatedVector;
    }
    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        timeSinceStart = 0;
        velocity = new Vector3(0f, 0f, 0f);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        gear = gearChangeUI.GetGear();
        if (gear != 0 && Input.GetKey("w"))
        {
            if(speed < maxSpeed *gear){
                 if(speed <= maxSpeed*gear){
                    speed += 2f * Time.deltaTime;
                 }
                rb.AddForce(transform.forward*speed);
            }
              
        }
        else{
            if(speed > 0 ){
                speed -= 2f;
                rb.AddForce(transform.forward*speed);
            }
        }
        if(Input.GetKey(KeyCode.Space)){
            if(Input.GetKeyDown("s")){
                speed -= 2f;
                rb.AddForce(transform.forward*speed);
            }
              if(speed > 0 ){
                speed -= (1/(1+gear));
                velocity = new Vector3(0f, 0f, -movementZ); 
                rb.AddForce(transform.forward*speed);
            } 
        }
        if (Input.GetKey("a"))
        {
            // if(rb.velocity.x > -speed){
            //     velocity = new Vector3(-speed, 0f, 0f);  
            //     rb.AddForce(velocity);
            //  }
           
            //transform.Rotate(0, 1, 0, Space.Self);
            float turn = -5f;
            rb.AddTorque(transform.up * Time.deltaTime * turn);
        }
       
        if (Input.GetKey("d"))
        {
            // if(rb.velocity.x < speed){
            //     velocity = new Vector3(speed, 0f, 0f);  
            //     rb.AddForce(velocity);
            //  } 
            
            //direction +=1;
            float turn = 5f;
            rb.AddTorque(transform.up * Time.deltaTime * turn);
            
            //transform.Rotate(0, -1, 0, Space.Self);
        }
       
        if (Input.GetKey("s"))
        {
            stall = true;     
        }
        
        //rb.AddForce(velocity * Time.deltaTime)
        //USE ADD FORCE WITH MOVEMENT Z AS CHECKED VARIABLES TO A MAXIMUM BASED ON VEHICLE  AND DECREASE TOWARDS ZERO WHEN NO DIRECTION IS APPLIED
    }
}
