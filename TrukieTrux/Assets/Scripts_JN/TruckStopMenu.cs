using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TruckStopMenu : MonoBehaviour
{
    [Header("Animations")]
    public Animator moneyButton;
    public Animator walletTab;
    public Animator nextButton;
    public Animator cameraDolly;
    public GameObject moneyParticleEffect;
    public float spawnRate;
    public int maxSpew = 100;
    public Transform moneySpawn;
    public GameObject screenWipe;

    [Header("Run Summary")]
    public GameObject summaryPanel;
    public GameObject walletPanel;

    [Header("Garage")]
    public GameObject garagePanels;
    public GameObject jobCards;
    public GameObject upgradePanel;


    bool jobsOpen = false;


    bool isSpewing = false;

    float lastSpewTime = 0f;

    int spews = 0;

    void Start()
    {
        summaryPanel.SetActive(true);
        walletPanel.SetActive(true);
    }

    public void OnMoneyButton()
    {
        moneyButton.SetTrigger("Clicked");
        
        isSpewing = true;
    }

    public void OnNextButton()
    {
        Instantiate(screenWipe, new Vector3(0f,0f,0f), Quaternion.identity, transform.parent);
        StartCoroutine(PanToGarage());
    }

    public void OnJobsButton()
    {
        if (!jobsOpen)
            jobCards.SetActive(true);
        else
            jobCards.SetActive(false);
    }


    void Update()
    {
        if (isSpewing)
        {
            
            if (Time.time > lastSpewTime + spawnRate)
            {
                float r = Random.Range(-25f, 25f);

                Vector3 spawnPos = new Vector3(moneySpawn.position.x, moneySpawn.position.y + r, moneySpawn.position.z);


                Instantiate(moneyParticleEffect, spawnPos, Quaternion.identity, moneySpawn);
                spews++;
                lastSpewTime = Time.time;

                if (spews >= maxSpew)
                {
                    isSpewing = false;
                    walletTab.SetTrigger("MoneyClick");
                }
            }

        }
    }

    IEnumerator PanToGarage()
    {
        yield return new WaitForSeconds(0.3f);

        cameraDolly.SetTrigger("NextScreen");
        summaryPanel.SetActive(false);
        walletPanel.SetActive(false);

        garagePanels.SetActive(true);

    }


}
