using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDepotHandler : MonoBehaviour
{
     
    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.GetComponent<TrukController>() != null)
        {

            Debug.Log("ENTER");

            StaticStats.remainingMass = other.gameObject.GetComponent<TrukController>().truckMass;
            StaticStats.timeElapsed = other.gameObject.GetComponent<TrukController>().timeElapsed;
            
            other.gameObject.GetComponent<TrukController>().WipeDownSpawn();

            GetComponent<SceneSwitch>().ToEndAnim();
        }

    }
}
