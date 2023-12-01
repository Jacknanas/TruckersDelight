using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticStats
{
    public static Run run { get; set; }
    public static TruckStats truckStats { get; set; }

    public static float remainingMass;
    public static int timeElapsed;

    public static AudioClip song;

    public static float volume = 1f;

    public static int masteredDiff = 2;

}
