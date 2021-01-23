using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject PauseMenuObject;
    public GameObject[] ObjectsToToggleOff;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            PauseMenuObject.SetActive(true);
        }
    }

    public void LeaveMenu()
    {
        Time.timeScale = 1;
        PauseMenuObject.SetActive(false);
        Debug.Log("Left Pause Menu");
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
        Debug.Log("returned TO Main Menu");
    }
}
