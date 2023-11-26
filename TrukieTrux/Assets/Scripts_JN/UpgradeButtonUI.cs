using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButtonUI : MonoBehaviour
{
    public Text titleText;
    public string title;
    public UpgradeType type;

    public Text level;
    public int currentLevel;

    public Text price;
    public int currentPrice;
    public int basePrice;
    public Text descriptionBox;
    [TextArea(5,15)]
    public string description;

    public GameObject infoButton;
    public GameObject buyButton;

    public TruckStopMenu menu;

    void Start()
    {
        level.text = currentLevel.ToString();
        price.text = currentPrice.ToString();
        titleText.text = title;
        buyButton.SetActive(GetAvailability(menu.stats.playerBalance));
    }



    public bool GetAvailability(int playerBalance)
    {
        return playerBalance >= currentPrice;
    }

    public void OnPurchase()
    {

        currentPrice *= 2; 
        currentLevel += 1;

        menu.Upgrade(type, currentPrice/2);

        level.text = currentLevel.ToString();
        price.text = currentPrice.ToString();

    }

    public void OnInfoButton()
    {
        descriptionBox.text = description;
    }
}

public enum UpgradeType{
    MaxSpeed,
    CargoSpace,
    Acceleration,
    Breaks,
    TurningPower,
    TurboForce,
    TruckType,
    Wildcard
}

