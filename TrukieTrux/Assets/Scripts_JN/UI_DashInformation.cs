using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_DashInformation : MonoBehaviour
{

    public Text speedText;
    public Color defaultColour;
    public Color maxColour;

    public Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        speedText.color = defaultColour;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSpeedIndicator(Rigidbody rb, bool isMax)
    {
        
        //camera.fov = 60 + speed / 90f;

        int speed = Mathf.FloorToInt(rb.velocity.magnitude);


        speedText.text = speed.ToString();
        if (isMax)
            speedText.color = maxColour;
        else
            speedText.color = defaultColour;

    }

}
