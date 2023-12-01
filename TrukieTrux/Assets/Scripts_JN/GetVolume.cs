using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetVolume : MonoBehaviour
{

    AudioListener listen;
    float vol = 1f;

    // Start is called before the first frame update
    void Start()
    {
        listen = GetComponent<AudioListener>();
    }

    // Update is called once per frame
    void Update()
    {

        vol = StaticStats.volume;

        AudioListener.volume = vol;
    }
}
