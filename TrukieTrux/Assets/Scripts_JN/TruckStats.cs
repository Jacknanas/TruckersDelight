using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckStats
{
    public int currentMaxSpeed = 4;
    public int cargoMax = 500;
    public float acceleration = 2.5f;
    public float breakDrag = 2f;
    public float turnPower = 600f;
    public float turboForce = 60f;
    public TruckType truck = TruckType.Pickup;
    
    public float speedModifier = 1f;

    public int playerBalance = 0;
    public int lifeTimeBalance = 0;

    public bool hasForceField = false;

    public void AcquireNewSpecial(string name)
    {
        SpecialUpgrade new_sp = GetSpecialUpgrade(name);

        new_sp.OnAcquired(this);

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