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
    public List<RunCardUI> runCards;

    [Header("Garage")]
    public GameObject garagePanels;
    public GameObject jobCards;
    public GameObject upgradePanel;

    [Header("PlayerInformation")]
    public int totalEarnings = 0;
    public int maxCargo = 500;
    public int truck = 1;
    public int speedLevel = 1;
    public int masteredDifficulty = 1;

    [Header("RunGeneration")]
    public List<string> firstNames;
    public List<string> lastNames;
    public float levelOneRunLength = 500f;
    public int levelOnePay = 100;

    bool jobsOpen = false;


    bool isSpewing = false;

    float lastSpewTime = 0f;

    int spews = 0;

    void Start()
    {
        summaryPanel.SetActive(true);
        walletPanel.SetActive(true);


        for (int i = 0; i<30; i++)
        {
            Debug.Log(NameOfRun(JobType.Milk));
        }

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
        {
            jobCards.SetActive(true);
            jobsOpen = true;
            
        }
        else
        {
            jobCards.SetActive(false);
            jobsOpen = false;
        }
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
        ProduceNewRuns(totalEarnings+1, truck);
    }


    public void ProduceNewRuns(int earnings, int truckLevel)
    {
        List<Run> runsToAdd = new List<Run>();

        for (int i = 0; i < 3; i++)  // to generate three runs
        {

            Run currentRun = GenerateRun(earnings, i, truckLevel);
            runsToAdd.Add(currentRun);

            runCards[i].SetCardInformation(currentRun);
        }
        

    }


    public Run GenerateRun(int earnings, int relativeDifficulty, int truckLevel)
    {

        float randomMode = Random.Range(0.5f, 1.5f);

        JobType jType = (JobType)Random.Range(GetMinTypeLevel(earnings,relativeDifficulty,truckLevel), GetMaxTypeLevel(relativeDifficulty,truckLevel));

        string title = NameOfRun(jType);

        int mass = Random.Range(GetMinMass(relativeDifficulty,maxCargo), GetMaxMass(relativeDifficulty,maxCargo));

        int length = GetRunLength(relativeDifficulty);

        int difficulty = GetRunDifficulty(relativeDifficulty);

        int time = Mathf.FloorToInt(30 * length * difficulty / speedLevel);

        int pay = GetPay(relativeDifficulty, difficulty, length, jType, mass);


        Run newRun = new Run(title, time, length, mass, difficulty, jType, pay);

        return newRun;
    }

    int GetMaxTypeLevel(int rd, int truckLevel)
    {
            
        if (rd == 0)
        {
            return 1 + truckLevel;
        }
        else if (rd == 1)
        {
            return 3 + truckLevel;
        }
        else
        {
            return 5 + truckLevel;
        }


    }
    int GetMinTypeLevel(int earnings, int rd, int truckLevel)
    {
            
        if (rd == 0)
        {
            return 0 + Mathf.FloorToInt(truckLevel / 3 + earnings / 1000000);
        }
        else if (rd == 1)
        {
            return 1 + Mathf.FloorToInt(truckLevel / 2 + earnings / 1000000);
        }
        else
        {
            return 2 + truckLevel;
        }


    }

    int GetMinMass(int rd, int mC)
    {
        if (rd == 0)
        {
            return Mathf.FloorToInt(mC * 0.4f);
        }
        else if (rd == 1)
        {
            return Mathf.FloorToInt(mC * 0.65f);
        }
        else
        {
            return Mathf.FloorToInt(mC * 0.8f);
        }
    }

    int GetMaxMass(int rd, int mC)
    {
        if (rd == 0)
        {
            return Mathf.FloorToInt(mC * 0.65f);
        }
        else if (rd == 1)
        {
            return Mathf.FloorToInt(mC * 0.8f);
        }
        else
        {
            return Mathf.FloorToInt(mC * 1f);
        }
    }

    int GetRunLength(int rd)
    {
        float truckMod = 3f / (truck+1f);
        float rng = Random.Range(-levelOneRunLength/truckMod, levelOneRunLength/truckMod);
    
        return Mathf.FloorToInt(levelOneRunLength + rng);
    }

    int GetRunDifficulty(int rd)
    {
        
        int d_l = masteredDifficulty;
        int d_u = masteredDifficulty;

        if (rd == 0)
        {
            d_l = masteredDifficulty - 5;
            d_u = masteredDifficulty - 1;
        }
        else if (rd == 1)
        {
            d_l = masteredDifficulty - 3;
            d_u = masteredDifficulty + 2;
        }
        else
        {
            d_l = masteredDifficulty;
            d_u = masteredDifficulty + 4;
        }

        d_l = Mathf.Clamp(d_l, 1, 23);
        d_u = Mathf.Clamp(d_l, 1, 23);

        return Random.Range(d_l, d_u);
    }

    int GetPay(int rd, int diff, int length, JobType type, int mass)
    {
        float typeMod = 1.0f;    

        if (type == JobType.Food)
        {
            typeMod = 1.0f;
        }
        else if (type == JobType.Parcels)
        {
            typeMod = 1.15f;
        }
        else if (type == JobType.Garbage)
        {
            typeMod = 1.4f;
        }
        else if (type == JobType.Furniture)
        {
            typeMod = 1.65f;
        }
        else if (type == JobType.Water)
        {
            typeMod = 2f;
        }
        else if (type == JobType.Milk)
        {
            typeMod = 2.3f;
        }
        else if (type == JobType.IceCream)
        {
            typeMod = 2.6f;
        }
        else if (type == JobType.Tech)
        {
            typeMod = 3f;
        }
        else if (type == JobType.Gas)
        {
            typeMod = 3.5f;
        }
        else
        {
            typeMod = 4f;
        }

        typeMod += (0.1f * rd);

        float pay = levelOnePay + (0.1f * diff * diff * levelOnePay) + (length/(levelOneRunLength*2f)) * (mass/levelOnePay);
        pay *= typeMod;

        return Mathf.FloorToInt(pay);


    }


    public string NameOfRun(JobType type)
    {
        string typeName = "little";

        int rng = Random.Range(0,2);

        if (type == JobType.Food)
        {
            if (rng == 0)
                typeName = "food";
            else
                typeName = "grocery";
        }
        else if (type == JobType.Parcels)
        {
            if (rng == 0)
                typeName = "parcel";
            else
                typeName = "package";
        }
        else if (type == JobType.Garbage)
        {
            if (rng == 0)
                typeName = "stinky";
            else
                typeName = "garbage";
        }
        else if (type == JobType.Furniture)
        {

            typeName = "furniture";
 
        }
        else if (type == JobType.Water)
        {
            if (rng == 0)
                typeName = "water";
            else
                typeName = "wet";
        }
        else if (type == JobType.Milk)
        {
            if (rng == 0)
                typeName = "milk";
            else
                typeName = "dairy";
        }
        else if (type == JobType.IceCream)
        {
            if (rng == 0)
                typeName = "frozen";
            else
                typeName = "ice cream";
        }
        else if (type == JobType.Tech)
        {
            if (rng == 0)
                typeName = "high-tech";
            else
                typeName = "super cyber";
        }
        else
        {
            if (rng == 0)
                typeName = "dangerous";
            else
                typeName = "high-stakes";
        }

        string forName = $"{firstNames[Random.Range(0,firstNames.Count)]} {lastNames[Random.Range(0,lastNames.Count)]}";

        string result = $"A {typeName} delivery for {forName}";

        return result;
    }


}
