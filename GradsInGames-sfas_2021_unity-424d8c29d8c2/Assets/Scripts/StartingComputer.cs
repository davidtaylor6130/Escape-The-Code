using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingComputer : MonoBehaviour
{
    public Transform PlayerStartingLocation;
    private ElectronicInteractable EI;

    // Start is called before the first frame update
    void Start()
    {
        //- Gets The Interactable Script and sets required data for computer exit -//
        EI = this.GetComponent<ElectronicInteractable>();
        EI.isControllingObject = true;
        EI.PlayersPreviousPosition = PlayerStartingLocation.position;
    }
}
