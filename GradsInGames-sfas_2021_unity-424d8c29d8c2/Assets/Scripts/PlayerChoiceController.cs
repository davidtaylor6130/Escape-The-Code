using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChoiceController : MonoBehaviour
{
    public float BitCoin;
    public float CarmaAmount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("BitCoin: " + BitCoin);
        Debug.Log("CarmaAmount: " + CarmaAmount);
    }
}
