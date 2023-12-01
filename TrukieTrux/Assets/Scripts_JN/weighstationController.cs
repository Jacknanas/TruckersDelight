using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class weighstationController : MonoBehaviour 
{
    public int scaleText = 0;

    [SerializeField]
    private PersistantData persistant;

    public GameObject cop;
    public Transform spawnPoint;

    Transform player;
    bool hasWeighed = false;

    bool postDelay = false;


    [SerializeField]
    private TMP_Text tmp;
    // Start is called before the first frame update
    void Start()
    {
        if(scaleText == 0){
            tmp.text = "00000000";
        }
        else{
            tmp.text = "12012414";
        }

        StartCoroutine(DelayPlayerFind());

    }


    IEnumerator DelayPlayerFind()
    {
        yield return new WaitForSeconds(1f);

        player = FindObjectOfType<TrukController>().transform;
        postDelay = true;
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<TrukController>() != null)
        {
            var mass = other.gameObject.GetComponent<TrukController>().truckMass;
            tmp.text = $"{mass} kg";
            hasWeighed = true;
        }

        /*
        if(persistant.getCargoWeight() == 0){
            tmp.text = "000000000";
        }
        else{
            tmp.text = persistant.getCargoWeight().ToString();
        }
        */
    }



    void FixedUpdate()
    {

        if (postDelay)
        {
            if (player.position.x > transform.position.x && !hasWeighed)
            {

                Debug.Log("Passed weigh");

                if (player.GetComponent<TrukController>().hasCB)
                {
                    if (UnityEngine.Random.Range(0.0f, 1.0f) < 0.66f)
                    {
                        SpawnCops(2);
                        
                    }
                    postDelay = false;
                }
                else
                {
                    SpawnCops(2);
                    postDelay = false;
                }

            }

        }

    }


    public void SpawnCops(int num)
    {
        Debug.Log("COPS A COMIN");

        for (int i = 0; i < num; i++)
        {
            GameObject coppy = Instantiate(cop, spawnPoint.position, Quaternion.identity);
            coppy.transform.Translate(i*6f, 0f, 0f);
        }
    }

}
