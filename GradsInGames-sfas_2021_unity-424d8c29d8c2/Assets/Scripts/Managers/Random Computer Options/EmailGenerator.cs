using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct EventActivationInformation
{
    public EventActivationInformation(int Action, int Name)
    {
        ActionIndex = Action;
        NameIndex = Name;
    }
    public int ActionIndex;
    public int NameIndex;
};

public struct ConstructedEmail
{
    public string NameOfEmail;
    [TextArea]
    public string OutputOfEmail;
};

public class EmailGenerator : MonoBehaviour
{
    [Header("Settings")]
    public int EmailsPerComputer;
    private int AmountOfComputers;

    [Header("Recorded User Activations")]
    public List<EventActivationInformation> RecorededActions;

    [Header("Developer Created Filler Emails")]
    public ConstructedEmail[] FillerEmails;
    public List<int> ActiveFillerEmails;

    [Header("Generated Emails Based Of User Pc Input")]
    public ConstructedEmail[] GeneratedEmails;
    public List<int> ActiveGeneratedEmails;

    public void CustomStart(int a_amountOfComputers)
    {
        AmountOfComputers = a_amountOfComputers;

        //- Initalise List To Store Information -//
        RecorededActions = new List<EventActivationInformation>();
    }

    public ConstructedEmail[] GenerateEmails()
    {
        GeneratedEmails = new ConstructedEmail[RecorededActions.Count];

        ConstructedEmail[] EmailsToReturn = new ConstructedEmail[EmailsPerComputer];

        //- if Actions Recored Construct Emails Based On Them -//
        if (RecorededActions.Count != 0)
        {
            //-----------------<--------------- Random Email In Here
        }

        //- Each Email Per Computer -//
        for (int i = 0; i < EmailsPerComputer; i++)
        {
            //- if there is no generated emails then just pull out of the random dev created pool -//
            if (GeneratedEmails.Length == 0)
            {
                EmailsToReturn[i] = FillerEmails[GetRandomFillerEmail()];
            }
            else
            {
                EmailsToReturn[i] = GeneratedEmails[GetRandomGeneratedEmail()];
            }
        }

        //- Allows next call to reslect same emails -//
        ClearRandomSelectionMemory();

        //- Returned Generated Emails -//
        return EmailsToReturn;
    }

    public void AddCompleatedEvent(int EventIndex, int nameIndex)
    {
        RecorededActions.Add(new EventActivationInformation(EventIndex, nameIndex));
    }

    public int GetRandomFillerEmail()
    {
        int randomSelection = 0;
        do
        {
            randomSelection = Random.Range(0, FillerEmails.Length);
        }
        while (ActiveFillerEmails.Contains(randomSelection));
        ActiveFillerEmails.Add(randomSelection);

        return randomSelection;
    }

    public int GetRandomGeneratedEmail()
    {
        int randomSelection = 0;
        do
        {
            randomSelection = Random.Range(0, GeneratedEmails.Length);
        }
        while (ActiveGeneratedEmails.Contains(randomSelection));
        ActiveGeneratedEmails.Add(randomSelection);

        return randomSelection;
    }

    public void ClearRandomSelectionMemory()
    {
        ActiveGeneratedEmails.Clear();
        ActiveFillerEmails.Clear();
    }

    public void ResetDay()
    {
        ActiveFillerEmails = new List<int>();
        ActiveGeneratedEmails = new List<int>();
    }
}
