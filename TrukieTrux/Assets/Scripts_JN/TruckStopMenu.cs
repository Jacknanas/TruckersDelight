using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TruckStopMenu : MonoBehaviour
{

    public Animator moneyButton;
    public GameObject moneyParticleEffect;
    public float spawnRate;
    public int maxSpew = 100;



    bool isSpewing = false;
    Transform buttonTransform;

    float lastSpewTime = 0f;

    int spews = 0;

    void Start()
    {
        buttonTransform = moneyButton.gameObject.transform;
    }

    public void OnMoneyButton()
    {
        moneyButton.SetTrigger("Clicked");
        isSpewing = true;
    }


    void Update()
    {
        if (isSpewing)
        {
            
            if (Time.time > lastSpewTime + spawnRate + Random.Range(-0.1f,0.3f))
            {
                Instantiate(moneyParticleEffect, buttonTransform.localPosition, Quaternion.identity, transform.parent);
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
