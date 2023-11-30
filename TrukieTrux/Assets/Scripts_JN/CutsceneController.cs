using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneController : MonoBehaviour
{

    public AudioSource screechs;

    public AudioSource rev;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TheScene());
    }
    

    IEnumerator TheScene()
    {
        screechs.Play();

        yield return new WaitForSeconds(2.5f);

        screechs.Stop();

        yield return new WaitForSeconds(0.1f);

        rev.Play();

        yield return new WaitForSeconds(4.3f);

        GetComponent<SceneSwitch>().ToMenu();
    }
}
