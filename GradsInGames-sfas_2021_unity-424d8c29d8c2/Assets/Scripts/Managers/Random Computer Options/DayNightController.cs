using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public struct PcEvent
{
    public string NameOfEvent;
    [TextArea]
    public string OutputOfEvent;
    public float NoticableCost;
    public float CarmaCost;
};

[System.Serializable]
public struct TextureRenderers
{
    public GameObject ParentObject;
    public TextMeshProUGUI OnScreenText;
    public Camera Camera;
    public Game StoryManager;
};

[System.Serializable]
public struct ActiveComputerInfo
{
    public int ComputerIndex;
    public List<int> EventIndex;
    public int RenderTextureIndex;
};

public class DayNightController : MonoBehaviour
{
    [Header("Settings")]
    [Range(1,10)] public int AmountOfComputersActive;
    [Range(1,5)] public int EventsPerSystem;

    [Header("PC Events")]
    public PcEvent[] Events;
    private List<int> ActiveEvents;

    [Header("Computer Selection")]
    public GameObject[] Computers;
    public List<int> ActiveComputers;

    [Header("Camera To Texture Systems")]
    public GameObject[] CameraToTexture;
    private TextureRenderers[] CamToTex;

    [Header("Final Output")]
    public ActiveComputerInfo[] FormattedComputers;


    // Start is called before the first frame update
    void Start()
    {
        //- Get Refrances to Camera To Texture Components -//
        GetCameraToTextureInfo();

        //- Calculate Days Computer's and events -//
        RefreshComputers();
    }

    [ContextMenu("Get Camera To Texture Info")]
    void GetCameraToTextureInfo()
    {
        CamToTex = new TextureRenderers[CameraToTexture.Length];

        for (int i = 0; i < CameraToTexture.Length; i++)
        {
            CamToTex[i].ParentObject = CameraToTexture[i];
            CamToTex[i].OnScreenText = CameraToTexture[i].GetComponentInChildren<TextMeshProUGUI>();
            CamToTex[i].Camera = CameraToTexture[i].GetComponentInChildren<Camera>();
            CamToTex[i].StoryManager = CameraToTexture[i].GetComponent<Game>();
        }
    }

    [ContextMenu("Refresh Computers")]
    void RefreshComputers()
    {
        //- Clears All Info About What is selected -//
        ResetToDay();

        //- Generate Required Information -//
        for (int i = 0; i < AmountOfComputersActive; i++)
        {
            //- PcInfo -//
            // What Computer To Active
            FormattedComputers[i].ComputerIndex = GetRandomComputer();
            
            // Select Amount Of Events
            for (int j = 0; j < EventsPerSystem; j++)
                FormattedComputers[i].EventIndex.Add(GetRandomEvent());
            
            // Link Render Text System
            FormattedComputers[i].RenderTextureIndex = i;

            //- Clear Active Events -//
            ActiveEvents.Clear();
        }
    }

    int GetRandomComputer()
    {
        int randomSelection = 0;
        do
        {
            randomSelection = Random.Range(0, Computers.Length);
        }
        while (ActiveComputers.Contains(randomSelection));
        ActiveComputers.Add(randomSelection);

        return randomSelection;
    }

    int GetRandomEvent()
    {
        int randomSelection = 0;
        do
        {
            randomSelection = Random.Range(0, Events.Length);
        }
        while (ActiveEvents.Contains(randomSelection));
        ActiveEvents.Add(randomSelection);

        return randomSelection;
    }

    void ResetToDay()
    {
        FormattedComputers = new ActiveComputerInfo[AmountOfComputersActive];
        ActiveComputers.Clear();

        foreach (GameObject temp in Computers)
        {
            temp.SetActive(false);
        }
    }
}