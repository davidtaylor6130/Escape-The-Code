using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public enum WinLossStates
{
    Good = 0,
    Neurtal,
    Bad,
    BatteryDeath,
    FoundDeath
};

public class WinLossState : MonoBehaviour
{
    public GameObject ParentGui;

    public GameObject[] WinlossCondition;

    // Start is called before the first frame update
    void Start()
    {
        ParentGui.SetActive(false);
    }

    public void SetWinLossCondition(WinLossStates state)
    {
        //- turn off all possible win loss texts -//
        foreach (GameObject StateOption in WinlossCondition)
        {
            StateOption.SetActive(false);
        }

        //- turn on correct one -//
        WinlossCondition[(int)state].SetActive(true);
        
        //- turn on entire gui window -//
        ParentGui.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        Debug.Log("returned TO Main Menu");
        SceneManager.LoadScene("MainMenuScene");
    }

    public void ExitGame()
    {
        Debug.Log("It Quit");
        Application.Quit();
    }
}
