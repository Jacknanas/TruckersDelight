using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpawner : MonoBehaviour
{
    public List<GameObject> truckControllers;

    public GearChangingUI gcUI;
    public UI_DashInformation ui_DI;
    public Text timeText;
    public Text targText;
    public Text countDown;
    public SplinesForRoad road;
    public GameObject restart;
    public RectTransform sG;
    public GameObject rW;

    // Start is called before the first frame update
    void Start()
    {
        

        int truckNum = 0;


        if (StaticStats.truckStats != null)
        {
            truckNum = (int)StaticStats.truckStats.truck;

            
        }


        TrukController player = Instantiate(truckControllers[truckNum], transform.position, transform.rotation).GetComponent<TrukController>();

        player.gearChangeUI = gcUI;
        player.dashInfo = ui_DI;
        player.timeText = timeText;
        player.targetTimeText = targText;
        player.countDownText = countDown;
        player.roadManager = road;
        player.restart = restart;
        player.stallGuage = sG;
        player.reverseWarning = rW;

    }


}
