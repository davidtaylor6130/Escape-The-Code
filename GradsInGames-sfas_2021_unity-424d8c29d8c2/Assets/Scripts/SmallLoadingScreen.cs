using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallLoadingScreen : MonoBehaviour
{
    public GameObject LoadingScreen;
    public GameObject[] ToToggleOff;
    private float timeLeft = 10;
    private bool stopCalled = false;

    private void Update()
    {
        if (timeLeft < 0 && stopCalled)
        {
            LoadingScreen.SetActive(false);
            stopCalled = false;

            foreach (GameObject toToggle in ToToggleOff)
                toToggle.SetActive(true);
        }
        else
        {
            timeLeft -= Time.deltaTime;
        }
    }

    public void StartLoadingScreen(float minTime)
    {
        timeLeft = minTime;

        LoadingScreen.SetActive(true);
        stopCalled = false;

        foreach (GameObject toToggle in ToToggleOff)
            toToggle.SetActive(false);
    }

    public void StopLoadingScreen()
    {
        stopCalled = true;
    }
}
