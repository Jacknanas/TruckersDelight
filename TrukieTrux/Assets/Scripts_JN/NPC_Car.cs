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

    float lastForce = 0f;
    bool isGoing = false;

    Rigidbody rb;

    public void Initiate(SplinesForRoad roader)
    {
        roadGen = roader;
        rb = GetComponent<Rigidbody>();

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
    void Update()
    {
        if (Time.time > lastForce + moveInterval && isGoing)
        {
            Vector3 target = targetPositions[GetClosestPosId()];

            Vector3 altTarget = new Vector3(target.x, transform.position.y, target.z + zDisplacement);

            Vector3 dir = altTarget - transform.position;

            dir = Vector3.Normalize(dir);

            rb.AddForce(dir * moveForce);

            transform.LookAt(altTarget);

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
