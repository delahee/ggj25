using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    private void Start()
    {
        
    }

    public void LoadGame()
    {
        var ass = SceneManager.LoadSceneAsync("Level",LoadSceneMode.Additive);
        StartCoroutine(WaitScene(ass));
    }

    IEnumerator WaitScene(AsyncOperation ass)
    {
        while (!ass.isDone)
        {
            yield return null; 
        }
        UnloadIntro();
    }
    public void UnloadIntro()
    {
        SceneManager.UnloadSceneAsync("Intro");
    }
}
