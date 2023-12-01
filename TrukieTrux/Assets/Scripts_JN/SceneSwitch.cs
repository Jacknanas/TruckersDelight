using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Go()
    {
        
        StartCoroutine(GoDelay());

        //StaticStats.run = //assign Run here
        //StaticStats.truckStats = //assign truckStats here
    }




    IEnumerator GoDelay()
    {
        yield return new WaitForSeconds(1.2f);
        SceneManager.LoadScene (sceneBuildIndex:2);
    }

    public void ToEndAnim()
    {
        StartCoroutine(EndDelay());
    }

    IEnumerator EndDelay()
    {
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene (sceneBuildIndex:3);
    }

    public void ToMenu()
    {
        SceneManager.LoadScene (sceneBuildIndex:1);
    }

    public void ToDie()
    {
        SceneManager.LoadScene (sceneBuildIndex:4);
    }


    public void Restart()
    {
        StaticStats.run = null;
        StaticStats.truckStats = null;
        SceneManager.LoadScene (sceneBuildIndex:1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
