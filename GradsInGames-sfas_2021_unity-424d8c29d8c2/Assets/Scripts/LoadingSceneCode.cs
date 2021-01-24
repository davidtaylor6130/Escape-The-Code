using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneCode : MonoBehaviour
{
    public Image LoadBar;
    public Animator transition;

    public string SceneToTransitionToo;
    public bool runAuto;

    // Start is called before the first frame update
    void Start()
    {
        if (runAuto)
            StartCoroutine(LoadScene());
    }

    public void buttonStart()
    {
        Time.timeScale = 1;
        StartCoroutine(LoadScene());
    }


    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(0.5f);

        transition.SetTrigger("FadeToBlack");

        yield return new WaitForSeconds(0.5f);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneToTransitionToo);
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            LoadBar.fillAmount = asyncLoad.progress;
            yield return new WaitForEndOfFrame();
        }
    }
}
