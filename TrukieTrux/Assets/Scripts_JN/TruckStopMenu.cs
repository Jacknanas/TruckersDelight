using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TruckStopMenu : MonoBehaviour
{
    [Header("Animations")]
    public Animator moneyButton;
    public Animator walletTab;
    public Animator jobsButton;
    public Animator upgradesButton;
    public Animator cameraDolly;
    public GameObject moneyParticleEffect;
    public float spawnRate;
    public int maxSpew = 100;
    public Transform moneySpawn;
    public GameObject screenWipe;
    public AudioSource buttonSounder;
    public List<AudioClip> buttonSounds;
    public AudioClip moneyButtonSound;

    public GameObject screenWipeDown;

    [Header("Run Summary")]
    public GameObject summaryPanel;
    public Run lastRun;
    public GameObject walletPanel;
    public List<RunCardUI> runCards;
    public Text payAmount;

    Run nextRun;

    [Header("Garage")]
    public GameObject garagePanels;
    public GameObject jobCards;
    public GameObject upgradePanel;
    public GameObject musicPanel;
    public List<UpgradeButtonUI> upgrades;
    public List<SpecialUpgrade_UI> specialUpgrades;
    public Text descriptionText;
    public Color jobsDefault;
    public Color goDefault;
    public Color greyedOut;
    public RunCardUI selectedRunSum;
    public Animator goButton;

    [Header("PlayerInformation")]
    public int totalEarnings = 0;
    public int maxCargo = 500;
    public int truck = 1;
    public int speedLevel = 1;
    public int masteredDifficulty = 1;
    public TruckStats stats;

    public Text playerMoneyTextSummaryScreen;
    public Text playerMoneyTextGarageScreen;

    public Text lifetimeTextGarageScreen;

    [Header("RunGeneration")]
    public List<string> firstNames;
    public List<string> lastNames;
    public float levelOneRunLength = 500f;
    public int levelOnePay = 100;

    [Header("Pay Mod")]
    public AnimationCurve massMod;
    public AnimationCurve timeMod;

    [Header("For Summary Screen")]
	public GameObject niceSticker;
	public GameObject excSticker;
	public GameObject yikesSticker;

    bool jobsOpen = false;
    bool upgradesOpen = false;
    bool isNextSumOpen = false;

    bool hasSelected = false;

    bool isSpewing = false;

    float lastSpewTime = 0f;

    int spews = 0;

    int toPay = 0;

    void Start()
    {
        //startWipeUp.SetBool("IsUp", true);
        

        goButton.gameObject.GetComponent<Image>().color = greyedOut;
        jobsButton.gameObject.GetComponent<Image>().color = jobsDefault;

        goButton.gameObject.GetComponent<Button>().enabled = false;

        if (StaticStats.run != null)
            lastRun = StaticStats.run;
        if (StaticStats.truckStats != null)
            stats = StaticStats.truckStats;

        if (stats == null)
        {
            stats = new TruckStats();
        }

        GetPlayerStats();

        if (lastRun != null)
        {

            toPay = GetPay();
            StaticStats.song = null;


            payAmount.text = $"{toPay} $";
            maxSpew = Mathf.FloorToInt(lastRun.pay / 200f) + 1;
        }
        else
        {
            int manualPay = 2000;
            payAmount.text = $"{manualPay} $";

            maxSpew = Mathf.FloorToInt(manualPay / 200f) + 1;
        }

        StartCoroutine(StartPanels());

    }

    IEnumerator StartPanels()
    {

        yield return new WaitForSeconds(0.8f);

        summaryPanel.SetActive(true);
        walletPanel.SetActive(true);
    }

    int GetPay()
    {
        float massRat = StaticStats.remainingMass / lastRun.mass;
        float timeRat = StaticStats.timeElapsed / lastRun.expectedTime;


        float massPostEv = massMod.Evaluate(massRat);
        float timePostEv = timeMod.Evaluate(timeRat);


        if (timeRat <= 1.01f && massRat >= 0.75f)
        {
            if (lastRun.difficulty > masteredDifficulty)
            {
                masteredDifficulty = lastRun.difficulty;
            }
        }

        return Mathf.FloorToInt(((massPostEv + timePostEv) / 2f) * lastRun.pay);

    }



    public void GetPlayerStats()
    {
        totalEarnings = stats.lifeTimeBalance;
        maxCargo = stats.cargoMax;
        truck = (int) stats.truck;
        speedLevel = stats.currentMaxSpeed;
    }


    public void OnMusicButton()
    {
        if (musicPanel.activeSelf)
        {
            musicPanel.SetActive(false);
        }
        else
        {
            musicPanel.SetActive(true);
        }
    }


    public void OnGoButton()
    {
        Animator wipeDown = Instantiate(screenWipeDown, new Vector3(0f,1111f,0f), Quaternion.identity, transform.parent).GetComponent<Animator>();
    }

    public void OnMoneyButton()
    {
        moneyButton.SetTrigger("Clicked");
        buttonSounder.clip = moneyButtonSound;
        buttonSounder.Play();
        isSpewing = true;

        moneyButton.gameObject.GetComponent<Button>().enabled = false;


        if (lastRun != null)
        {
            stats.playerBalance += toPay;
            stats.lifeTimeBalance += toPay;

            if (toPay / lastRun.pay >= 1f)
            {
                excSticker.SetActive(true);
            }
            else if (toPay / lastRun.pay >= 0.7f)
            {
                niceSticker.SetActive(true);
            }
            else if (toPay / lastRun.pay <= 0.5f)
            {
                yikesSticker.SetActive(true);
            }
        }
        else
        {
            stats.playerBalance += 2000;
            stats.lifeTimeBalance += 2000;

        }

    }

    public void OnNextButton()
    {
        PlayButtonSound();

        if (stats != null)
        {
            if (stats.spUpgrades.Count > 0)
            {
                for (int i = 0; i < stats.spUpgrades.Count; i++)
                {
                    specialUpgrades[i].AddUpgrade(stats.spUpgrades[i], upgrades[0].descriptionBox);

                }

            }
        }

        Instantiate(screenWipe, new Vector3(0f,0f,0f), Quaternion.identity, transform.parent);
        StartCoroutine(PanToGarage());
    }

    public void OnJobsButton()
    {
        PlayButtonSound();

        jobsButton.SetTrigger("OnClick");

        if (!jobsOpen)
        {
            jobCards.SetActive(true);
            jobsOpen = true;

            if (upgradesOpen)
            {
                upgradePanel.SetActive(false);
                upgradesOpen = false;
            }

            
        }
        else
        {
            jobCards.SetActive(false);
            jobsOpen = false;
        }
    }

    public void OnUpgradesButton()
    {
        PlayButtonSound();

        upgradesButton.SetTrigger("OnClick");

        if (!upgradesOpen)
        {
            upgradePanel.SetActive(true);
            upgradesOpen = true;
            
            if (jobsOpen)
            {
                jobCards.SetActive(false);
                jobsOpen = false;
            }
            if (isNextSumOpen)
            {
                selectedRunSum.gameObject.SetActive(false);
                isNextSumOpen = false;
            }
        }
        else
        {
            upgradePanel.SetActive(false);
            upgradesOpen = false;

            if (!isNextSumOpen && hasSelected)
            {
                selectedRunSum.gameObject.SetActive(true);
                isNextSumOpen = true;
            }
        }

        



    }

    public void SelectedRun(Run runToAdd)
    {
        PlayButtonSound();

        nextRun = runToAdd;

        selectedRunSum.SetCardInformation(nextRun);

        selectedRunSum.gameObject.SetActive(true);
        isNextSumOpen = true;
        hasSelected = true;

        goButton.gameObject.GetComponent<Image>().color = goDefault;
        jobsButton.gameObject.GetComponent<Image>().color = greyedOut;

        goButton.gameObject.GetComponent<Button>().enabled = true;
        goButton.gameObject.GetComponent<Animator>().enabled = true;


        StaticStats.run = nextRun;
    }

    void PlayButtonSound()
    {
        int randomSoundNum = Random.Range(0, buttonSounds.Count);

        buttonSounder.clip = buttonSounds[randomSoundNum];

        buttonSounder.Play();
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

        playerMoneyTextSummaryScreen.text = $"{stats.playerBalance} $";
        playerMoneyTextGarageScreen.text = $"{stats.playerBalance} $";
        lifetimeTextGarageScreen.text = $"{stats.lifeTimeBalance} $";

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

    public void Upgrade(UpgradeType type, int price)
    {
        PlayButtonSound();
        stats.playerBalance -= price;
        //int typeNum = (int) type;

        if (type == UpgradeType.MaxSpeed)  //this will add one gear
        {
            stats.currentMaxSpeed++;
            stats.speedLevel++;
        }
        else if (type == UpgradeType.CargoSpace) 
        {
            stats.cargoMax = Mathf.FloorToInt(stats.cargoMax + stats.cargoMax * 0.05f);
            stats.cargoLevel++;
        }
        else if (type == UpgradeType.Acceleration) 
        {
            stats.acceleration = stats.acceleration + stats.acceleration * 0.05f;
            stats.accLevel++;
        }
        else if (type == UpgradeType.Breaks) 
        {
            stats.breakDrag = stats.breakDrag + stats.breakDrag * 0.05f;
            stats.breakLevel++;
        }
        else if (type == UpgradeType.TurningPower) 
        {
            stats.turnPower = stats.turnPower + stats.turnPower * 0.05f;
            stats.turnLevel++;
        }
        else if (type == UpgradeType.TurboForce) 
        {
            stats.turboForce = stats.turboForce + stats.turboForce * 0.05f;
            stats.turbLevel++;
        }
        else if (type == UpgradeType.TruckType) 
        {
            int truckAsNum = (int) stats.truck;

            stats.truck = (TruckType) truckAsNum+1;
            stats.truckLevel++;
        }
        else //wildcard
        {
            int random_up_ind = Random.Range(0, SpecialUpgradeDB.SpecialUpgrades.Count);

            string nameOfUpgrade = SpecialUpgradeDB.SpecialUpgrades[random_up_ind].Name;

            stats.AcquireNewSpecial(nameOfUpgrade);

            bool placed = false;
            for (int i = 0; i < specialUpgrades.Count; i++)
            {
                if (!specialUpgrades[i].isFull && !placed)
                {
                    specialUpgrades[i].AddUpgrade(SpecialUpgradeDB.SpecialUpgrades[random_up_ind], descriptionText);
                    placed = true;
                }

            }

        }

        AssessUpgradeAvailabilities();
        GetPlayerStats();

        StaticStats.truckStats = stats;
    }

    public void InitializeUpgradeUI(UpgradeType type, UpgradeButtonUI upgradeUI)
    {

        if (type == UpgradeType.MaxSpeed)  //this will add one gear
        {
            upgradeUI.currentLevel = stats.speedLevel;
        }
        else if (type == UpgradeType.CargoSpace) 
        {
            upgradeUI.currentLevel = stats.cargoLevel;
        }
        else if (type == UpgradeType.Acceleration) 
        {
            upgradeUI.currentLevel = stats.accLevel;
        }
        else if (type == UpgradeType.Breaks) 
        {
            upgradeUI.currentLevel = stats.breakLevel;
        }
        else if (type == UpgradeType.TurningPower) 
        {
            upgradeUI.currentLevel = stats.turnLevel;
        }
        else if (type == UpgradeType.TurboForce) 
        {
            upgradeUI.currentLevel = stats.turbLevel;
        }
        else if (type == UpgradeType.TruckType) 
        {
            upgradeUI.currentLevel = stats.truckLevel;
        }
        else //wildcard
        {
            upgradeUI.currentLevel = stats.spUpgrades.Count + 1;

        }

    }

    void AssessUpgradeAvailabilities()
    {
        foreach (UpgradeButtonUI upg in upgrades)
        {

            bool isGood = upg.GetAvailability(stats.playerBalance);

            if (!isGood)
            {
                upg.buyButton.SetActive(false);
            }
            else
            {
                upg.buyButton.SetActive(true);
            }

        }
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

        Debug.Log($"Diff {difficulty}");

        int time = Mathf.FloorToInt(50f * length / (speedLevel*130f) * (5f / (4f + relativeDifficulty))) + 75;

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
        
        int d_l = 0;
        int d_u = 0;

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
            d_l = masteredDifficulty + 1;
            d_u = masteredDifficulty + 3;
        }

        d_l = Mathf.Clamp(d_l, 1, 23);
        d_u = Mathf.Clamp(d_u, 1, 23);

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
