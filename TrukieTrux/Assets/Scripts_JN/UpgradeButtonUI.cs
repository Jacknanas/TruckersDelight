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
        menu.InitializeUpgradeUI(type, this);

        currentPrice = CalculatePrice(currentLevel);

        level.text = currentLevel.ToString();
        price.text = currentPrice.ToString();
        titleText.text = title;
        buyButton.SetActive(GetAvailability(menu.stats.playerBalance));
    }



    public bool GetAvailability(int playerBalance)
    {

        if (type == UpgradeType.Wildcard && currentLevel == 5)
            return false;
        else if (type == UpgradeType.TruckType && currentLevel == 6)
            return false;
        else if (type == UpgradeType.MaxSpeed && currentLevel == 13)
            return false;

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

    int CalculatePrice(int level)
    {
        int price = basePrice;

        for (int i = 0; i < level; i++)
        {
            price *= 2;
        }

        return price;
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

