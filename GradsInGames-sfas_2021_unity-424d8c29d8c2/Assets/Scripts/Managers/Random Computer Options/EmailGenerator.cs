using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EventActivationInformation
{
    public EventActivationInformation(int Action, int NameAndPc)
    {
        ActionIndex = Action;
        NameAndPcIndex = NameAndPc;
    }
    public int ActionIndex;
    public int NameAndPcIndex;
};

[System.Serializable]
public struct Email
{
    public string NameOfEmail;
    [TextArea]
    public string OutputOfEmail;
};

[System.Serializable]
public struct EmailTemplate
{
    public EmailTemplateFormat[] EmailTemplates;
};

[System.Serializable]
public struct EmailTemplateFormat
{
    public string To;
    public string From;
    public string Subject;
    [TextArea]
    public string Content;
    public string Attachments;
};

public class EmailGenerator : MonoBehaviour
{
    [Header("Ref To other Generators")]
    public NameGenerator NameGen;

    [Header("Settings")]
    public int EmailsPerComputer;
    private int AmountOfComputers;

    [Header("Recorded User Activations")]
    public EventActivationInformation[,] RecorededActions;

    [Header("Developer Filler Emails")]
    public Email[] FillerEmails;
    public List<int> ActiveFillerEmails;

    [Header("Generated Emails Based Of User Pc Input")]
    public EmailTemplate[] emailTemplateForAutoGeneration;
    [Space(10)]
    public Email[,] GeneratedEmails;
    public List<int> ActiveGeneratedEmails;

    public void CustomStart(int a_amountOfComputers)
    {
        AmountOfComputers = a_amountOfComputers;

        //- Initalise List To Store Information -//
        RecorededActions = new EventActivationInformation[AmountOfComputers, 2];

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                RecorededActions[i, j].ActionIndex = -1;
                RecorededActions[i, j].NameAndPcIndex = -1;
            }
        }
    }

    [ContextMenu("GenerateEmails")]
    public void GenerateEmails()
    {
        GeneratedEmails = new Email[AmountOfComputers, EmailsPerComputer];

        for (int i = 0; i < AmountOfComputers; i++)
        {

            //- loop through possible events -//
            for (int j = 0; j < EmailsPerComputer; j++)
            {
                if (j != 2)
                {
                    if (RecorededActions[i, j].ActionIndex == -1 && RecorededActions[i, j].NameAndPcIndex == -1)
                    {
                        //- Set Random Filler Email -//
                        GeneratedEmails[i, j] = FillerEmails[GetRandomFillerEmail()];
                    }
                    else
                    {
                        //- Create Temp Email Storage -//
                        Email tempEmail = new Email();

                        //- Select Randomly what email template to use for this action -//
                        int EmailVersionIndex = Random.Range(0, 3);

                        //- Get Email Name and Output -//
                        tempEmail = EmailTemplateToEmail(emailTemplateForAutoGeneration[RecorededActions[i, j].ActionIndex].EmailTemplates[EmailVersionIndex], i);

                        //- Save Email To Array For Later Use -//
                        GeneratedEmails[i, j] = tempEmail;
                    }
                }
                else if (j == 2)
                {
                    GeneratedEmails[i, j] = FillerEmails[GetRandomFillerEmail()];
                }
            }

            //- Allows next call to reslect same emails -//
            ClearRandomSelectionMemory();
        }
    }
    
    public string GetEmailInfo(int TitleOrBody, int PcIndex, int EmailIndex)
    {
        if (TitleOrBody == 0)
            return GeneratedEmails[PcIndex - 1, EmailIndex - 1].NameOfEmail;
        else
            return GeneratedEmails[PcIndex - 1, EmailIndex - 1].OutputOfEmail;
    }

    public Email EmailTemplateToEmail(EmailTemplateFormat template, int PcIndex)
    {
        Email Temp = new Email();
        //- Generate Email Name -//
        Temp.NameOfEmail = template.Subject + " From: " + NameGen.GetName(PcIndex);

        //- Generate Email Output-//
        Temp.OutputOfEmail = "> " + (template.To != "" ? template.To : NameGen.GetNameExcluding(PcIndex)) + "\n" +
                            "> From: " + (template.From != "" ? template.From : NameGen.GetName(PcIndex)) + "\n" +
                            "> Subject: " + template.Subject + "\n" +
                            "> Content: " + template.Content + "\n" +
                            "> Attachments: " + template.Attachments + "\n" +
                            "\n" + "Press Esc To Return To Menu";
        return Temp;
    }

    public void AddCompleatedEvent(int PcIndex, int EventIndex)
    {
        //- Can do a max of -//
        RecorededActions[(PcIndex - 1), (EventIndex)] = new EventActivationInformation(PcIndex, EventIndex);
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

    public void ClearRandomSelectionMemory()
    {
        ActiveGeneratedEmails.Clear();
        ActiveFillerEmails.Clear();
    }

    public void ResetDay()
    {
        ActiveFillerEmails = new List<int>();
        ActiveGeneratedEmails = new List<int>();
        GenerateEmails();
    }
}
