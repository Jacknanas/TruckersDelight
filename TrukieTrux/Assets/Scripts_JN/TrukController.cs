using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrukController : MonoBehaviour
{
    private Vector3 velocity;
    public int gear = 0;
    public int lastGear = 0;
    int newGear = 0;
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

    public GameObject restart;
    public float turn = 10f;

    public float breakSpeed = 0.5f;


    // public Vector3 RotateVectorAroundY(float angleDegrees, Vector3 originalVector)
    // {
    //     // Convert angle to radians
    //     float angleRadians = Mathf.Deg2Rad * angleDegrees;

    //     // Create a quaternion for the rotation around the Y axis
    //     Quaternion rotationQuaternion = Quaternion.Euler(0, angleDegrees, 0);

    //     // Apply the rotation to the original vector
    //     Vector3 rotatedVector = rotationQuaternion * originalVector;

    //     return rotatedVector;
    // }


    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        timeSinceStart = 0;
        velocity = new Vector3(0f, 0f, 0f);
        restart.SetActive(false);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!stall){
        
            newGear = gearChangeUI.GetGear(); // Take Gear from the Shifter
            if(gear != newGear)
            {
                Debug.Log("Last Gear"+ lastGear);
                Debug.Log("Gear"+ lastGear);
                Debug.Log("New Gear"+ newGear);

                if(gear !=0 )
                {
                    lastGear = gear;
                }
                gear = newGear;
            }
            if(lastGear!=0 && (gear - lastGear) > 1){
                stall = true; // Stalled their rig through overshifting
                restart.SetActive(true);
            }

            if (gear != 0 && !Input.GetKey(KeyCode.Space) && Input.GetKey("w"))
            {
                if(speed < maxSpeed *gear){
                    if(speed <= maxSpeed*gear){
                        speed += 2f * Time.deltaTime;
                    }
                    
                }
                
            }
            if(!Input.GetKey("w")){ // coast
                if(gear != 0){
                    if(speed > 0)
                    {
                        speed -= (1/(1*gear));
                    }
                }
                else{
                     if(speed > 0)
                    {
                        speed -= (1/(1*(1+gear)));
                    }
                }
            }
            if(Input.GetKey(KeyCode.Space)){
                if(Input.GetKeyDown("s")){ // break
                    if(speed > 0 ){
                        if(breakSpeed * gear > speed)
                        {
                            speed = 0;
                        }
                        else{
                            speed -= breakSpeed * gear;
                        }
                        
                        
                    }
                } 
                else{//Gear = 0 Clutch is In
                    if(Time.deltaTime > speed)
                    {
                        speed = 0;
                    }
                    else{
                            speed -= Time.deltaTime;
                    }
                        
                }
                
            }
            else{
                if(!Input.GetKey("w")){
                    if(Time.deltaTime > speed)
                    {
                        speed = 0;
                    }
                    else{
                            speed -= Time.deltaTime;
                    }
                }
            }
            if (Input.GetKey("a"))
            {
                // if(rb.velocity.x > -speed){
                //     velocity = new Vector3(-speed, 0f, 0f);  
                //     rb.AddForce(velocity);
                //  }
            
                //transform.Rotate(0, 1, 0, Space.Self);
                //float turn = -10f;

                rb.AddTorque(transform.up * Time.deltaTime * -turn);
            }
        
            if (Input.GetKey("d"))
            {
                // if(rb.velocity.x < speed){
                //     velocity = new Vector3(speed, 0f, 0f);  
                //     rb.AddForce(velocity);
                //  } 
                
                //direction +=1;
                //float turn = 10f;
                rb.AddTorque(transform.up * Time.deltaTime * turn);
                
                //transform.Rotate(0, -1, 0, Space.Self);
            }
        
            if (speed!=0 && !Input.GetKey(KeyCode.Space) && Input.GetKey("s")) // You cant press the Break in gear
            {
                stall = true;     
            }
            Debug.Log("Speed: " + speed);
            rb.AddForce(transform.forward*speed);
            //rb.AddForce(velocity * Time.deltaTime)
            //USE ADD FORCE WITH MOVEMENT Z AS CHECKED VARIABLES TO A MAXIMUM BASED ON VEHICLE  AND DECREASE TOWARDS ZERO WHEN NO DIRECTION IS APPLIED
        }
        else{
            restart.SetActive(true);
            if (Input.GetKey(KeyCode.Space) && Input.GetKey("s") && Input.GetKey("r"))
            {
                restart.SetActive(false);
                stall = false;     
            }
        }
    }
}
