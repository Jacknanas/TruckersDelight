using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    

    public float direction = 90;
    
    [Header("Other Objects")]
    public GearChangingUI gearChangeUI;
    public UI_DashInformation dashInfo;
    public Transform particleSpawn;
    public GameObject driveParticle;

    public Transform boostParticleSpawn;
    public GameObject boostParticle;

    public Text timeText;
    public Text targetTimeText;

    public Text countDownText;

    public SplinesForRoad roadManager;

    public GameObject screenWipeDown;

    [Header("Driving Variables")]
    public float torque = 1;
    public float maxSpeed = 10f;
    public float acceleration = 2f;
    public float turn = 10f;
    public float breakSpeed = 0.5f;
    public float boostForce = 100f;
    public AnimationCurve accCurve;
    public AnimationCurve turnCurve;
    public float boostLength = 2.5f;
    public LayerMask road;


    [Header("Truck Variables")]
    public float truckMass;
    public float gravity;
    public float truckHeight;
    public float breakDrag;

    bool hasForceField = false;
    bool hasTruckersCap = false;

    [Header("Audio")]
    public AudioSource revIdle;
    public AudioSource turboRev;
    public AudioClip idleSound;
    public AudioClip revSound;

    float currentMaxSpeed = 0f;
    bool wasMaxSpeed = false;

    public int timeElapsed;

    bool isReversing = false;

    bool isWaiting = true;

    float speedModifier = 1f;

    public GameObject restart;

    float boostTime = 0f;
    bool isBoost = false;

    float startTime = 0f;

    List<Vector3> middleVerts;


    // Use this for initialization
    void Start()
    {

        StartCoroutine(StartDelay());

        maxSpeed *= speedModifier;

        rb = GetComponent<Rigidbody>();
        timeSinceStart = 0;
        velocity = new Vector3(0f, 0f, 0f);
        restart.SetActive(false);

        revIdle.loop = true;

        if (StaticStats.run != null)
            ExtractRunData();
        if (StaticStats.truckStats != null)
            ExtractTruckData();
    }


    void ExtractRunData()
    {
        Run run = StaticStats.run;

        truckMass = run.mass;
        targetTimeText.text = run.expectedTime.ToString();
    }

    void ExtractTruckData()
    {
        TruckStats stats = StaticStats.truckStats;

        acceleration = stats.acceleration;
        breakDrag = stats.breakDrag;
        turn = stats.turnPower;
        boostForce = stats.turboForce;
        speedModifier = stats.speedModifier;
    }


    IEnumerator StartDelay()
    {
        countDownText.text = "10";
        yield return new WaitForSeconds(1f);
        countDownText.text = "9";
        yield return new WaitForSeconds(1f);
        countDownText.text = "8";
        yield return new WaitForSeconds(1f);
        countDownText.text = "7";
        yield return new WaitForSeconds(1f);
        countDownText.text = "6";
        yield return new WaitForSeconds(1f);
        countDownText.text = "5";
        yield return new WaitForSeconds(1f);
        countDownText.text = "4";
        yield return new WaitForSeconds(1f);
        countDownText.text = "3";
        yield return new WaitForSeconds(1f);
        countDownText.text = "2";
        yield return new WaitForSeconds(1f);
        countDownText.text = "1";
        middleVerts = roadManager.GetMiddles();
        Debug.Log($"GOT: {middleVerts.Count} from dumdum");
        yield return new WaitForSeconds(1f);
        countDownText.text = "GO";
        isWaiting = false;
        startTime = Time.time;
        yield return new WaitForSeconds(1f);
        countDownText.gameObject.SetActive(false);
        

        
    }

    void OnForceField(float strength, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        foreach (var hitCollider in hitColliders)
        {
            Vector3 forceDir = Vector3.Normalize(hitCollider.gameObject.transform.position - transform.position);
            
            if (hitCollider.gameObject.GetComponent<Rigidbody>() != null)
                hitCollider.gameObject.GetComponent<Rigidbody>().AddForce(forceDir * strength);
        }
    }

    float GetGroundMod()
    {
        if (AssessOnRoad())
        {
            //Debug.Log("on road");
            return 1f;
        }
        else if (hasTruckersCap)
        {
            return 0.75f;
        }
        //Debug.Log("OFF road");
        return 0.5f;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (isWaiting)
            return;
        
        timeElapsed = Mathf.FloorToInt(Time.time - startTime);

        timeText.text = timeElapsed.ToString();

        //speed *= GetGroundMod();

        if (isBoost)
        {
            rb.AddForce(transform.forward * boostForce * gear / 2f);

            if (Time.time > boostTime + boostLength)
            {
                isBoost = false;
            }

        }

        if (hasForceField && Input.GetKey("f"))
        {
            OnForceField(90f, 30f);
        }


        if (isReversing)
        {
            if (Input.GetKey("s") && rb.velocity.magnitude < 8f)
            {
                rb.AddForce(transform.forward*-20f);
                
            }

            speed = 0f;

        }

        if (speed > currentMaxSpeed - 3f)
        {
            wasMaxSpeed = true;
        }
        else if (gear != 0 )
        {
            wasMaxSpeed = false;
        }

        if(!stall){


            if(gear !=0 )
            {
                currentMaxSpeed = (maxSpeed * gear);
                revIdle.pitch = 0.5f + speed / (currentMaxSpeed*2f);
                
            }
    
            newGear = gearChangeUI.GetGear(); // Take Gear from the Shifter
            if(gear != newGear)
            {
                Debug.Log("Last Gear"+ lastGear);
                Debug.Log("Gear"+ lastGear);
                Debug.Log("New Gear"+ newGear);

                if(gear !=0 )
                {
                    lastGear = gear;
                    currentMaxSpeed = (maxSpeed * gear);
                }
                
                gear = newGear;


                if(lastGear!=0 && (gear - lastGear) > 1){
                    stall = true; // Stalled their rig through overshifting
                    restart.SetActive(true);
                }

                else if (wasMaxSpeed && gear !=0 && lastGear != 0) // BOOST ON SWITCH AT MAX SPEED
                {
                    rb.AddForce(transform.forward * boostForce * gear);

                    isBoost = true;

                    turboRev.Play();
                    turboRev.pitch = UnityEngine.Random.Range(0.65f, 1.5f);
                    revIdle.Play();

                    Instantiate(boostParticle, boostParticleSpawn.position, Quaternion.identity, boostParticleSpawn).GetComponent<PartileLife>().Die();
                }


            }


            if (gear != 0 && !Input.GetKey(KeyCode.Space) && Input.GetKey("w") && !isReversing) // DRIVE
            {
                if(speed < maxSpeed *gear){
                    if(speed <= maxSpeed*gear)
                    {
                        var speedRel = (maxSpeed * gear) - speed;


                        speed += (acceleration + 5f/gear) * Time.deltaTime * accCurve.Evaluate(speed / maxSpeed * gear);
                    }
                    
                }
                
                if (Random.Range(1, 100) < 0.3 * maxSpeed*gear){
                    Instantiate(driveParticle, particleSpawn.position, Quaternion.identity);

                }
                

            }
            if(!Input.GetKey("w") || Input.GetKey(KeyCode.Space) && Input.GetKey("w")){ // coast
                if(gear != 0){
                    if(speed > 0)
                    {
                        speed -= (1/(1*gear)) * Time.deltaTime * acceleration;
                    }
                }
                else{
                    if(speed > 0)
                    {
                        speed -= (1/(1*(1+gear)))* Time.deltaTime * acceleration;
                    }
                }
            }
            if(Input.GetKey(KeyCode.Space)){
                

                if(Input.GetKey("s")){ // break
                    
                    rb.drag = breakDrag;

                    speed = rb.velocity.magnitude;

                    if(speed > 0 ){

                        //speed = 0;
                        if(breakSpeed * gear > speed)
                        {
                            speed = 0;
                        }
                        else{
                            speed -= breakSpeed * gear * gear * Time.deltaTime;   

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
                    if(Time.deltaTime * acceleration > speed)
                    {
                        speed = 0;
                    }
                    else{
                        speed -= (Time.deltaTime * acceleration);
                    }
                }

                rb.drag = 1f;
            }
            if (Input.GetKey("a") && rb.velocity.magnitude > 0.05f)
            {

                rb.AddTorque(transform.up * Time.deltaTime * -turn * turnCurve.Evaluate(rb.velocity.magnitude / 100f)*2f);
            }
        
            if (Input.GetKey("d") && rb.velocity.magnitude > 0.05f)
            {
                rb.AddTorque(transform.up * Time.deltaTime * turn * turnCurve.Evaluate(rb.velocity.magnitude / 100f)*2f);
                
            }
        
            if (speed!=0 && !Input.GetKey(KeyCode.Space) && Input.GetKey("s") && !isReversing) // You cant press the Break in gear
            {
                stall = true;     
                speed = 0f;
            }
            //Debug.Log("Speed: " + speed);
            dashInfo.SetSpeedIndicator(rb, speed > currentMaxSpeed - 3f);
            rb.AddForce(transform.forward*speed * GetGroundMod());

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


    void Update()
    {
        //RaycastHit hit;
            
        //if (!Physics.Raycast(transform.position, -transform.up, out hit, truckHeight))
        //{
        //    rb.AddForce(transform.up * - gravity * truckMass);
        //}
    }

    public void OnReverseButton()
    {
        if (isReversing)
        {
            isReversing = false;
        }
        else
            isReversing = true;
    }

    bool AssessOnRoad()
    {
        return roadManager.roadWidth+2f >= Mathf.Abs(Vector3.Distance(transform.position, middleVerts[GetClosestPosId()]));
    }



    int GetClosestPosId()
    {

        for (int i = 0; i < middleVerts.Count; i++)
        {
                    
            if (middleVerts[i].x > transform.position.x)
            {
                return i;
            }
        }

        return 0;
    }


    public void WipeDownSpawn()
    {
        Instantiate(screenWipeDown, new Vector3(0f,1111f,0f), Quaternion.identity, transform.parent).GetComponent<Animator>();
    }

}
