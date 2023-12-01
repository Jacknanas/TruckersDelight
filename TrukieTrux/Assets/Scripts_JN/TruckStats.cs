using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckStats
{
    public int currentMaxSpeed = 4;
    public int cargoMax = 500;
    public float acceleration = 2.5f;
    public float breakDrag = 1.6f;
    public float turnPower = 620f;
    public float turboForce = 60f;
    public TruckType truck = TruckType.Pickup;
    
    public List<SpecialUpgrade> spUpgrades = new List<SpecialUpgrade>();

    //LEVELS
    public int speedLevel = 1;
    public int cargoLevel = 1;
    public int accLevel = 1;
    public int breakLevel = 1;
    public int turnLevel = 1;
    public int turbLevel = 1;
    public int truckLevel = 1;


    public float speedModifier = 1f;

    public int playerBalance = 0;
    public int lifeTimeBalance = 0;

    public bool hasForceField = false;
    public bool hasTruckersCap = false;
    public bool hasCB = false;




    public void AcquireNewSpecial(string name)
    {
        SpecialUpgrade new_sp = GetSpecialUpgrade(name);

        new_sp.OnAcquired(this);

        spUpgrades.Add(new_sp);
    }

    SpecialUpgrade GetSpecialUpgrade(string id)
    {
        foreach (SpecialUpgrade sp in SpecialUpgradeDB.SpecialUpgrades)
		{
			if (sp.Name == id)
			{
				return sp;
			}
		}
        return null;
    }

}

public enum TruckType{
    Pickup,
    Moving,
    Flatbead,
    Transport,
    Gas
}