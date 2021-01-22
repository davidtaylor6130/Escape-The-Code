using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PcEvent
{
    public string NameOfEvent;
    [TextArea]
    public string OutputOfEvent;
    public float NoticableCost;
    public float CarmaCost;
};

public class EventGenerator : MonoBehaviour
{
    [Header("PC Events")]
    public PcEvent[] Events;
    private List<int> ActiveEvents;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void ResetDay()
    {
       
    }
}
