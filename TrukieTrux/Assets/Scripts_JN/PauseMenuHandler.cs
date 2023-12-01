using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuHandler : MonoBehaviour
{
    
    public Slider mainMenuAudioSlider;


    public bool isStartMenu;


    public GameObject pauseMenu;
    public GameObject controlsMenu;

    void Update()
    {
        if (isStartMenu)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.activeSelf)
            {
                pauseMenu.SetActive(false);
            }
            else
                pauseMenu.SetActive(true);
        }

    }

    public void ChangeVolume()
    {
        StaticStats.volume = mainMenuAudioSlider.value;
    }


    public void OnQuit()
    {
        Application.Quit();
    }

    public void OnResume()
    {
        pauseMenu.SetActive(false);
    }

    public void OnControls()
    {
        if (controlsMenu.activeSelf)
        {
            controlsMenu.SetActive(false);
        }
        else
            controlsMenu.SetActive(true);
    }

}
