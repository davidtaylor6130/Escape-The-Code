﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameGenerator : MonoBehaviour
{
    [Header("Settings")]
    public int NamesPerComputer = 1;
    private int AmountOfComputers;

    [Header("Names To Randomly Select From")]
    public string[] names;

    public void CustomStart(int a_amountOfComputers)
    {
        AmountOfComputers = a_amountOfComputers;
    }

    public string GetName(int index)
    {
        return names[index];
    }
}

//- This is a seprate class to allow for faster and easier devleopment in the future -//
