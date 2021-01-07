using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameStates : MonoBehaviour
{
    [Header("Cinemachine Infomation")]
    public CinemachineVirtualCamera[] VMCams;
    private CinemachineVirtualCamera CurrentActive;
    // Start is called before the first frame update
    void Start()
    {
        foreach (CinemachineVirtualCamera temp in VMCams)
        {
            temp.enabled = false;
        }

        //- First Camera is the Player Camera -//
        VMCams[1].enabled = true;
        CurrentActive = VMCams[1];
    }

    public void SetNewActiveCamera(CinemachineVirtualCamera a_newVMCam)
    {
        //- disable current vmCam -//
        CurrentActive.enabled = false;
        //- SetNew Cam and activate it -//
        CurrentActive = a_newVMCam;
        CurrentActive.enabled = true;
    }

    public void SetPlayerCameraActive()
    {
        CurrentActive.enabled = false;
        //- SetNew Cam and activate it -//
        CurrentActive = VMCams[0];
        CurrentActive.enabled = true;
    }
}
