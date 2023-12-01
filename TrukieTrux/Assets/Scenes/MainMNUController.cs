using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMNUController : MonoBehaviour
{
    public GameObject sliderMenu;
    public GameObject screenWipeDown;

    public void WipeDownSpawn()
    {
        Instantiate(screenWipeDown, new Vector3(0f,1111f,0f), Quaternion.identity, sliderMenu.transform.parent);
    }

    public void StarGame(){

        StartCoroutine(Delay());
        
       
    }


    public void OnSettingsButton()
    {
        if (sliderMenu.activeSelf)
            sliderMenu.SetActive(false);
        else
            sliderMenu.SetActive(true);
    }


    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.5f);

        WipeDownSpawn();

        yield return new WaitForSeconds(1.3f);
        SceneManager.LoadScene (sceneBuildIndex:1);
    }

}
