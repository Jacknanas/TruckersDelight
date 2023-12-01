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
				truckStats.cargoMax += Mathf.FloorToInt(truckStats.cargoMax * 0.12f);
				truckStats.breakDrag += truckStats.breakDrag * 0.12f;
				//BUT
				truckStats.speedModifier -= truckStats.speedModifier * 0.02f;
			},

			Name = "Super Suspension",
			Description = "Super Suspension: Increases cargo and breaking by 12%, but reduces overall speed by 2%",
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
				truckStats.turboForce += truckStats.turboForce * 0.3f;
			},

			Name = "NOS",
			Description = "NOS: Turbo force is increased by 30%.",
		}
		
		},

		{
		new SpecialUpgrade()
		{

			OnAcquired = (TruckStats truckStats) =>
			{
				truckStats.hasTruckersCap = true;
			},

			Name = "Trucker's Cap",
			Description = "Trucker's Cap: speed decrease from off-roading is halved.",
		}
		
		},

		{
		new SpecialUpgrade()
		{

			OnAcquired = (TruckStats truckStats) =>
			{
				truckStats.hasCB = true;
			},

			Name = "CB Radio",
			Description = "CB Radio: chance of police chasing is reduced by 33%.",
		}
		
		},

		{
		new SpecialUpgrade()
		{

			OnAcquired = (TruckStats truckStats) =>
			{
				truckStats.turnPower += truckStats.turnPower * 0.1f;
				truckStats.speedModifier += truckStats.speedModifier * 0.05f;
			},

			Name = "Drifter's Sunglasses",
			Description = "Drifer's Sunglasses: steering is increased by 10%, overall speed is increased by 5%",
		}
		
		},

		{
		new SpecialUpgrade()
		{

			OnAcquired = (TruckStats truckStats) =>
			{
				truckStats.cargoMax += Mathf.FloorToInt(truckStats.cargoMax * 0.3f);
			},

			Name = "Big Bed",
			Description = "Big Bed: cargo space is increased by 30%.",
		}
		
		},
    };
}
