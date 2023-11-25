using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class weighstationController : MonoBehaviour 
{
    public int scaleText = 120000;
    [SerializeField]
    private TMP_Text gameObject;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.text = scaleText.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
