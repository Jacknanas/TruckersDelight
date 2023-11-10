using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrukController : MonoBehaviour
{
    private Vector3 velocity;
    public int gear = 1;
    public float speed = 2f;
    private Rigidbody rb;
    private float movementX;
    private float movementZ = 2.0f;
    private float timeSinceStart;
    private bool stall = false;
    
    public float maxSpeed = 10f;
    
    public GearChangingUI gearChangeUI;


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
            if(rb.velocity.z < maxSpeed *gear){
                movementZ = speed * gear;
                velocity = new Vector3(0f, 0f, movementZ); 
                rb.AddRelativeForce(velocity * Time.deltaTime);
            }
              
        }
        else{
            if(rb.velocity.z > 0 ){
                movementZ = speed * (1/1+gear);
                velocity = new Vector3(0f, 0f, -movementZ); 
                rb.AddForce(velocity * Time.deltaTime);
            }
        }
        if(Input.GetKey(KeyCode.Space)){
            if(Input.GetKeyDown("s")){
                velocity = new Vector3(0f, 0f, -10f);
                rb.AddForce(velocity);
            }
              if(rb.velocity.z > 0 ){
                movementZ = speed * (1/1+gear);
                velocity = new Vector3(0f, 0f, -movementZ); 
                rb.AddForce(velocity * Time.deltaTime);
            } 
        }
        if (Input.GetKey("a"))
        {
            if(rb.velocity.x > -speed){
                velocity = new Vector3(-speed, 0f, 0f);  
                rb.AddForce(velocity);
             }
        }
        else{
            rb.velocity = new Vector3(0f, rb.velocity.y, rb.velocity.z);
        }
        if (Input.GetKey("d"))
        {
            if(rb.velocity.x < speed){
                velocity = new Vector3(speed, 0f, 0f);  
                rb.AddForce(velocity);
             } 
        }
        else{
            rb.velocity = new Vector3(0f, rb.velocity.y, rb.velocity.z);
        }
        if (Input.GetKey("s"))
        {
            stall = true;     
        }
        
        //rb.AddForce(velocity * Time.deltaTime)
        //USE ADD FORCE WITH MOVEMENT Z AS CHECKED VARIABLES TO A MAXIMUM BASED ON VEHICLE  AND DECREASE TOWARDS ZERO WHEN NO DIRECTION IS APPLIED
    }
}
