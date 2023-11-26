using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialUpgradeDB : MonoBehaviour
{
    public static List<SpecialUpgrade> SpecialUpgrades { get; set; } = new List<SpecialUpgrade>()
    {
        {
		new SpecialUpgrade()
		{

			OnAcquired = (TruckStats truckStats) =>
			{
				truckStats.cargoMax += Mathf.FloorToInt(truckStats.cargoMax * 0.1f);
				truckStats.breakDrag += truckStats.breakDrag * 0.1f;
				//BUT
				truckStats.speedModifier -= truckStats.speedModifier * 0.02f;
			},

			Name = "Super Suspension",
			Description = "Super Suspension: Increases cargo and breaking by 10%, but reduces overall speed by 2%",
		}
		
		},

		{
		new SpecialUpgrade()
		{

			OnAcquired = (TruckStats truckStats) =>
			{
				truckStats.turboForce += truckStats.turboForce * 0.07f;
				truckStats.speedModifier += truckStats.speedModifier * 0.07f;
				truckStats.acceleration += truckStats.acceleration * 0.07f;
			},

			Name = "Oil Change",
			Description = "Oil Change: Increases turbo, overall speed, and acceleration by 7%",
		}
		
		},

		{
		new SpecialUpgrade()
		{

			OnAcquired = (TruckStats truckStats) =>
			{
				truckStats.hasForceField = true;
			},

			Name = "Force Field",
			Description = "Force Field: Grants the ability to create a force field by pressing f",
		}
		
		},

		{
		new SpecialUpgrade()
		{

			OnAcquired = (TruckStats truckStats) =>
			{
				truckStats.speedModifier += truckStats.speedModifier * 0.08f;
				truckStats.acceleration += truckStats.acceleration * 0.08f;
			},

			Name = "Rockin Transmission",
			Description = "Rockin Transmission: Overall speed, and acceleration are increased by 8%",
		}
		
		},

		{
		new SpecialUpgrade()
		{

			OnAcquired = (TruckStats truckStats) =>
			{
				truckStats.turboForce += truckStats.turboForce * 0.2f;
			},

			Name = "NOS",
			Description = "NOS: Turbo force is increased by 20%.",
		}
		
		},

		{
		new SpecialUpgrade()
		{

			OnAcquired = (TruckStats truckStats) =>
			{
				// somehow code this
			},

			Name = "Trucker's Cap",
			Description = "Trucker's Cap: speed decrease from off-roading is halved.",
		}
		
		},
    };
}
