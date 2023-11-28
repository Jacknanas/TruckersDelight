using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearBoxBuilder : MonoBehaviour
{
    public float yCoordinateOrigin;
    public float xCoordinateOrigin;

    public float yIncrement;
    public float xIncrement;

    public Transform gearContainer;

    public Rigidbody2D handleRb;
    public GearChangingUI gearChange;

    [SerializeField] GameObject UpDownRight;
    [SerializeField] GameObject Down;
    [SerializeField] GameObject UpDownLeft;
    [SerializeField] GameObject UpDownLeftRight;
    [SerializeField] GameObject UpLeft;

    [Range(4, 16)]
    public int numGears = 4;

    // Start is called before the first frame update
    void Start()
    {
        if (StaticStats.truckStats != null)
            ExtractTruckData();


        GenerateGears(numGears);
        //handleRb.gameObject.transform.position = new Vector3(xCoordinateOrigin, yCoordinateOrigin, 0f);
    }

    void ExtractTruckData()
    {
        numGears = StaticStats.truckStats.currentMaxSpeed;
    }


    public void GenerateGears(int numberToGen)
    {
        int x_inc_counter = 0;
        int x_inc_mult = 0;

        for (int i = 0; i < numberToGen; i++)
        {

            x_inc_counter++;

            if (x_inc_counter == 2)
            {
                x_inc_counter = 0;
                x_inc_mult++;
            }   
            
            if (i == 0)
            {
                GameObject newslot = Instantiate(UpDownRight, new Vector3(xCoordinateOrigin, yCoordinateOrigin, 0f), Quaternion.identity, gearContainer);
                newslot.transform.localPosition = new Vector3(xCoordinateOrigin, yCoordinateOrigin, 0f);
                x_inc_counter--;

                InitializeSlot(newslot.GetComponentInChildren<SlotYanker>(), i);

            }


            else if (i % 2 != 0) // odd on bottom (even gear num though)
            {
                GameObject newslot = Instantiate(Down, new Vector3(xCoordinateOrigin + xIncrement*x_inc_mult, yCoordinateOrigin + yIncrement, 0f), Quaternion.identity, gearContainer);
                newslot.transform.localPosition = new Vector3(xCoordinateOrigin + xIncrement*x_inc_mult, yCoordinateOrigin + yIncrement, 0f);
                InitializeSlot(newslot.GetComponentInChildren<SlotYanker>(), i);

            }

            else
            {
                if (i < numberToGen-2)
                {
                    GameObject newslot = Instantiate(UpDownLeftRight, new Vector3(xCoordinateOrigin + xIncrement*x_inc_mult, yCoordinateOrigin, 0f), Quaternion.identity, gearContainer);
                    newslot.transform.localPosition = new Vector3(xCoordinateOrigin + xIncrement*x_inc_mult, yCoordinateOrigin, 0f);
                    InitializeSlot(newslot.GetComponentInChildren<SlotYanker>(), i);
                }
                else if (i == numberToGen-2)
                {
                    GameObject newslot = Instantiate(UpDownLeft, new Vector3(xCoordinateOrigin + xIncrement*x_inc_mult, yCoordinateOrigin, 0f), Quaternion.identity, gearContainer);
                    newslot.transform.localPosition = new Vector3(xCoordinateOrigin + xIncrement*x_inc_mult, yCoordinateOrigin, 0f);
                    InitializeSlot(newslot.GetComponentInChildren<SlotYanker>(), i);
                }
                else if (i == numberToGen-1)
                {
                    GameObject newslot = Instantiate(UpLeft, new Vector3(xCoordinateOrigin + xIncrement*x_inc_mult, yCoordinateOrigin, 0f), Quaternion.identity, gearContainer);
                    newslot.transform.localPosition = new Vector3(xCoordinateOrigin + xIncrement*x_inc_mult, yCoordinateOrigin, 0f);
                    InitializeSlot(newslot.GetComponentInChildren<SlotYanker>(), i);
                }

            }


        }



    }


    void InitializeSlot(SlotYanker newYank, int c_g)
    {
        newYank.gearChange = gearChange;
        newYank.handleRb = handleRb;
        newYank.gear = c_g+1;
    }


}
