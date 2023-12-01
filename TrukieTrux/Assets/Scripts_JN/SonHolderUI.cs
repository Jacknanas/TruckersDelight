using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SonHolderUI : MonoBehaviour
{
    public AudioClip song;

    public AudioSource pressed;

    public void OnSelected()
    {
        StaticStats.song = song;
        pressed.Play();
    }


}
