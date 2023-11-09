using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Animation : MonoBehaviour
{

    public bool playOnStart = true;
    public Image image;
    public Sprite[] sprites;
    public float frameTime = 0.25f;

    bool isPlaying = false;

    bool currentlyPlaying = false;


    int currentFrame = 0;

    // Start is called before the first frame update
    void Start()
    {  

        if (playOnStart)
        {
            isPlaying = true;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
            
        if (isPlaying && !currentlyPlaying)
        {
            StartCoroutine(NextFrame());

        }


    }

    IEnumerator NextFrame()
    {
        image.sprite = sprites[currentFrame];

        currentlyPlaying = true;


        yield return new WaitForSeconds(frameTime);

        if (currentFrame == sprites.Length - 1)
            currentFrame = 0;
        else
            currentFrame++;

        currentlyPlaying = false;
    }


}
