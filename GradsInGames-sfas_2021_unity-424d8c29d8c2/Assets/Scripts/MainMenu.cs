using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("It Quit");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("LoadScreen");
        Debug.Log("It Started");
    }
}
