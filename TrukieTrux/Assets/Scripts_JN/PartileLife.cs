using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartileLife : MonoBehaviour
{
    public float time;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Die());
    }
    
    public IEnumerator Die()
    {
        yield return new WaitForSeconds(time);
       
        Destroy(gameObject);
    }
}
