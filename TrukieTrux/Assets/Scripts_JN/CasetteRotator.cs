using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasetteRotator : MonoBehaviour
{
    float yRotation = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        yRotation = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
       
        transform.Rotate(0.0f, yRotation, 0.0f, Space.World);
    }
    void OnTriggerEnter(Collider other)
    {
        
    }
}
