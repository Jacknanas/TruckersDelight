using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class weighstationController : MonoBehaviour 
{
    public int scaleText = 0;

    [SerializeField]
    private PersistantData persistant;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(persistant.getCargoWeight() == 0){
            tmp.text = "000000000";
        }
        else{
            tmp.text = persistant.getCargoWeight().ToString();
        }
    }
}
