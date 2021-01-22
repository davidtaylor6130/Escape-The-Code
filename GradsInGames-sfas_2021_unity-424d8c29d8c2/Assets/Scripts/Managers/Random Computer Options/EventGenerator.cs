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
    [Header("Settings")]
    public int EventsPerComputer;
    private int AmountOfComputers;

    [Header("PC Events")]
    public PcEvent[] Events;
    private List<int> ActiveEvents;

    public void CustomStart(int a_amountOfComputers)
    {
        AmountOfComputers = a_amountOfComputers;
    }

    public int GetRandomEvent()
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

    public void ClearRandomSelectedMemory()
    {
        ActiveEvents.Clear();
    }

    public void ResetDay()
    {
        ActiveEvents = new List<int>();
    }
}