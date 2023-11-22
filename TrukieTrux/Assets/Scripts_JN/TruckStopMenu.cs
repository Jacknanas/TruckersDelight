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
    public GameObject moneyParticleEffect;
    public float spawnRate;
    public int maxSpew = 100;
    public Transform moneySpawn;


    bool isSpewing = false;

    float lastSpewTime = 0f;

    int spews = 0;

    void Start()
    {

    }

    public void OnMoneyButton()
    {
        moneyButton.SetTrigger("Clicked");
        walletTab.SetTrigger("MoneyClick");
        isSpewing = true;
    }

    public void OnNextButton()
    {
        moneyButton.SetTrigger("Clicked");
        isSpewing = true;
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
                }
            }

        }
    }

}
