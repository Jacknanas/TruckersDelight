using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GearChangingUI : MonoBehaviour
{

    public Animator handleAnimator;
    public Rigidbody2D handleRB;
    
    public float mouseDetectDistance = 60f;
    public float yankDistance = 50f;
    public float yankForce = 20f;
    
    public bool isClutch = false;

    public Text gearDisplay;

    public AudioClip gearIn;
    public AudioClip gearOut;
    public AudioSource sounderIn;
    public AudioSource sounderOut;

    int currentGearNumber = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))    
        {
            handleAnimator.SetBool("IsClutch", true);
            isClutch = true;

            currentGearNumber = 0;

            //sounderOut.clip = gearOut;
            sounderOut.pitch = Random.Range(0.7f, 1.1f);
            sounderOut.Play();

        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            handleAnimator.SetBool("IsClutch", false);
            isClutch = false;
            
        }


        if (isClutch)
        {
            if (Input.GetMouseButton(0))
            {
                var worldMousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
 


                var direction = worldMousePosition - handleRB.gameObject.GetComponent<RectTransform>().position;
                
                if (direction.magnitude <= mouseDetectDistance)
                    handleRB.AddForce(direction * 20f);
            }
        }

        gearDisplay.text = currentGearNumber.ToString();


    }

    public void SetGear(int id)
    {
        currentGearNumber = id;
        //sounderIn.clip = gearIn;
        sounderIn.pitch = Random.Range(0.6f, 1.0f);
        sounderIn.Play();
        
    }

    public int GetGear(){
        return currentGearNumber;
    }


}
