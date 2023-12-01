using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuHandler : MonoBehaviour
{
    
    public Slider mainMenuAudioSlider;


    public bool isStartMenu;

    void Start()
    {
    }



    public void ChangeVolume()
    {
        StaticStats.volume = mainMenuAudioSlider.value;
    }
}
