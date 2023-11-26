using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SpecialUpgrade : MonoBehaviour
{
    public string Name { get; set; }
    public string Description { get; set; }
    
    public Action<TruckStats> OnAcquired { get; set; }
}
