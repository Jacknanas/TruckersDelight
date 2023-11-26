using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistantData : MonoBehaviour
{

public int topSpeed = 0;
public int cargoWeight = 1000;
public int songsCollected = 0;

    // Start() and Update() methods deleted - we don't need them right now

    public static PersistantData Instance;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public int getCargoWeight(){
        return cargoWeight;
    }
    public int getSongsCollected(){
        return songsCollected;
    }

    public void CollectSong(){
        songsCollected += 1;
    }
}
