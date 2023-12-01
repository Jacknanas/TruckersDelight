using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicGrabber : MonoBehaviour
{
    AudioSource songPlayer;
    
    public List<AudioClip> defaultSongs;

    // Start is called before the first frame update
    void Start()
    {

        songPlayer = GetComponent<AudioSource>();

        if (StaticStats.song != null)
        {
            songPlayer.clip = StaticStats.song;
        }
        else
        {
            songPlayer.clip = defaultSongs[Random.Range(0, defaultSongs.Count)];
        }

        songPlayer.Play();

    }

}
