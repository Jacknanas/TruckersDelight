using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotYanker : MonoBehaviour
{
    
    public Rigidbody2D handleRb;
    public GearChangingUI gearChange;
    public int gear;

    RectTransform handleTransform;

    bool isAlreadySet = false;


    // Start is called before the first frame update
    void Start()
    {
        handleTransform = handleRb.gameObject.GetComponent<RectTransform>();

        Text numText = GetComponentInChildren<Text>();
        numText.text = gear.ToString();

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        var dir = GetComponent<RectTransform>().position - handleTransform.position;

        float mod = 1.0f;

        if (dir.magnitude <= gearChange.yankDistance)
        {

            if (dir.magnitude <= 8f)
            {
                handleRb.velocity = new Vector3(0f,0f,0f);
                
                if (!gearChange.isClutch && !isAlreadySet)
                {
                    gearChange.SetGear(gear);
                    isAlreadySet = true;
                    
                }
            }
            else
            {
                isAlreadySet = false;
            }

            dir = Vector3.Normalize(dir);

            if (!gearChange.isClutch)
                mod = 7f;

            handleRb.AddForce(dir * gearChange.yankForce * mod);
        }
    }
}
