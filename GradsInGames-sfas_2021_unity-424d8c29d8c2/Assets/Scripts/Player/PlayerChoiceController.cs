using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerChoiceController : MonoBehaviour
{
    public float BitCoin;

    public float CarmaAmount = 0;
    public float globalCarmaAmount = 0;

    public float NoticableNess = 0;

    public Slider GuiIndicator;

    // Update is called once per frame
    void Update()
    {
        Debug.Log("BitCoin: " + BitCoin);
        Debug.Log("CarmaAmount: " + CarmaAmount);
    }

    public void NoticeablenessChange(float amountToAdd)
    {
        NoticableNess += amountToAdd;
        GuiIndicator.value = (NoticableNess / 100);
    }

    public void ResetDay()
    {
        globalCarmaAmount += CarmaAmount;
        CarmaAmount = 0;
        NoticableNess = 0;
        NoticeablenessChange(0);
    }
}
