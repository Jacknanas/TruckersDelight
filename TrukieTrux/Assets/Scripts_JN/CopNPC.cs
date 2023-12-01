using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopNPC : MonoBehaviour
{

    Transform player;

    [Header("Parameters")]
    public float moveForce;
    public float waitTime;
    public float ramDist = 20f;
    public float reverseDist = 6f;
    public float ramMod = 2f;

    bool isGoing = false;

    int tooCloseCounter = 0;


    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        

        rb = GetComponent<Rigidbody>();

        StartCoroutine(SpawnWait());

    }

    IEnumerator SpawnWait()
    {
        yield return new WaitForSeconds(waitTime);
        player = FindObjectOfType<TrukController>().transform;
        isGoing = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isGoing)
        {
            transform.LookAt(player);

            RaycastHit hit;

            if (!Physics.Raycast(transform.position, transform.forward, out hit, ramDist))
            {
                rb.AddForce(transform.forward * moveForce * Random.Range(0.8f, 1.2f));
                
            }
            
            else
            {
                rb.AddForce(transform.forward * moveForce * ramMod);
            }

            if (Vector3.Distance(player.position, transform.position) < 12.5f)
            {
                player.GetComponent<TrukController>().truckMass--;
            }
            else if (Vector3.Distance(player.position, transform.position) > 60f)
            {
                rb.AddForce(transform.forward * moveForce * ramMod * Random.Range(0.1f, 0.3f));
            }

        }

    }
}
