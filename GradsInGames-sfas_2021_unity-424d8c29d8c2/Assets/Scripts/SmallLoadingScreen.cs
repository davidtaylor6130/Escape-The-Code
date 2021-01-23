using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallLoadingScreen : MonoBehaviour
{
    public GameObject LoadingScreen;
    public GameObject[] ToToggleOff;
    private float timeLeft = 0;
    private bool stopCalled = false;

    private void Update()
    {
        timeLeft -= Time.deltaTime;

        if (timeLeft < 0 && stopCalled)
        {
            LoadingScreen.SetActive(false);
            stopCalled = false;

            foreach (GameObject toToggle in ToToggleOff)
                toToggle.SetActive(true);
        }
    }

    public void StartLoadingScreen(float minTime)
    {
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
