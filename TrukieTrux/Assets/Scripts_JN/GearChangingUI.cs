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

    public TrukController truckController;
    public GameObject reverseBlocker;
    public GameObject reverseButton;

    int currentGearNumber = 0;

    bool isReversing = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isReversing)    
        {
            handleAnimator.SetBool("IsClutch", true);
            isClutch = true;

            currentGearNumber = 0;

            //sounderOut.clip = gearOut;
            sounderOut.pitch = Random.Range(0.7f, 1.1f);
            sounderOut.Play();

        }

        if (Input.GetKeyUp(KeyCode.Space) && !isReversing)
        {
            handleAnimator.SetBool("IsClutch", false);
            isClutch = false;
            
        }


        if (isClutch && !isReversing)
        {
            if (Input.GetMouseButton(0))
            {
                var worldMousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
 


                var direction = Vector3.Normalize(worldMousePosition - handleRB.gameObject.GetComponent<RectTransform>().position);
                
                if (direction.magnitude <= mouseDetectDistance)
                    handleRB.AddForce(direction * 4700f);
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
        
        if (currentGearNumber > 1)
        {
            reverseButton.SetActive(false);
        }
        else
            reverseButton.SetActive(true);

    }

    public int GetGear(){
        return currentGearNumber;
    }

    public void OnReverseButton()
    {
        truckController.OnReverseButton();

        if (isReversing)
        {
            reverseBlocker.SetActive(false);
            isReversing = false;
        }
        else
        {
            reverseBlocker.SetActive(true);
            isReversing = true;
        }
    }
}
