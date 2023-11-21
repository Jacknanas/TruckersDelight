using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartileLife : MonoBehaviour
{
    public float time;

    public bool isUIRotateParticle  = false;

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

    void Update()
    {
        if (isUIRotateParticle)
        {
            transform.Rotate(0f, 0f, 5f * Time.deltaTime);
        }
    }


}
