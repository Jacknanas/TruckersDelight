using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Car : MonoBehaviour
{

    List<Vector3> targetPositions;
    List<float> targetXPositions;

    public SplinesForRoad roadGen;

    [Header("Car Parameters")]

    public float moveForce;
    public float moveInterval;
    public float waitTime;
    public float zDisplacement;
    public float carDist = 5f;
    public float passChance = 0.01f;
    public LayerMask carIgnore;

    public bool isZigZagger = false;
    Vector3 lastTarget;

    float lastForce = 0f;
    bool isGoing = false;
    bool isRap = false;
    bool isCarAhead = false;

    Rigidbody rb;


    float initZDisp;


    public void Initiate(SplinesForRoad roader)
    {
        roadGen = roader;
        rb = GetComponent<Rigidbody>();

        initZDisp = zDisplacement;

        StartCoroutine(SpawnWait());

        //RecieveRoadInformation(roadGen.PositionsForNPC());

    }

    IEnumerator SpawnWait()
    {
        yield return new WaitForSeconds(waitTime);

        List<Vector3> positions = roadGen.PositionsForNPC();

        RecieveRoadInformation(positions);
        isGoing = true;
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.time > lastForce + moveInterval && isGoing)
        {
            Vector3 target = targetPositions[GetClosestPosId()];

            Vector3 altTarget = new Vector3(target.x, transform.position.y, target.z + zDisplacement);

            Vector3 dir = altTarget - transform.position;

            dir = Vector3.Normalize(dir);


            transform.LookAt(altTarget);

            //lastForce = Time.time;

            RaycastHit hit;

            // Does the ray intersect any objects excluding the player layer
            if (!Physics.Raycast(transform.position, transform.forward, out hit, carDist, carIgnore))
            {
                rb.AddForce(dir * moveForce);
                
            }
            else
            {
                isCarAhead = true;
            }

        }

        if (isCarAhead)
        {
            float rng = Random.Range(0.000f,1.000f);

            if (rng < passChance && zDisplacement == initZDisp)
            {
                zDisplacement = 0f;
                isCarAhead = false;
            }
            else if (rng < passChance && zDisplacement != initZDisp)
            {
                zDisplacement = initZDisp;
                isCarAhead = false;
            }

        }





        if (isRap)
        {
            transform.Translate(Vector3.up * Time.deltaTime * 5f);
            transform.Translate(Vector3.forward * Time.deltaTime * 0.7f);
            transform.Rotate(0f, 0f, 1.5f* Time.deltaTime);

            //rb.AddTorque(Vector3.up * Time.deltaTime * 1f);

            if (transform.position.y > 90f)
            {
                Destroy(gameObject);
            }

        }

        if (transform.position.x > targetPositions[targetPositions.Count-1].x-5.5f)
            Rapturize();


    }

    int GetClosestPosId()
    {

        for (int i = 0; i < targetPositions.Count; i++)
        {
                    
            if (targetPositions[i].x > transform.position.x)
            {
                

                if (lastTarget != targetPositions[i] && isZigZagger)
                {
                    SwitchSide();
                }

                lastTarget = targetPositions[i];

                return i;
            }
        }

        return 0;
    }


    void SwitchSide()
    {
        zDisplacement = -zDisplacement;
    }


    public void RecieveRoadInformation(List<Vector3> pos)
    {
        targetPositions = pos;

    }


    void Rapturize()
    {
        isGoing = false;
        isRap = true;

    }
}
