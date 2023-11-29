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

    float lastForce = 0f;
    bool isGoing = false;

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
            if (!Physics.Raycast(transform.position, transform.forward, out hit, carDist))
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


    }

    int GetClosestPosId()
    {

        for (int i = 0; i < targetPositions.Count; i++)
        {
                    
            if (targetPositions[i].x > transform.position.x)
            {
                return i;
            }
        }

        return 0;
    }


    public void RecieveRoadInformation(List<Vector3> pos)
    {
        targetPositions = pos;

        Debug.Log($"count: {targetPositions.Count}");
        /*for (int i = 0; i < pos.Count; i++)
        {
            targetXPositions.Add(pos[i].x);

        }*/
    }

}
