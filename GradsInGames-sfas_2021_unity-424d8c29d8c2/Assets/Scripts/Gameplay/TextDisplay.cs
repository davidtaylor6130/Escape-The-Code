using System.Collections;
using UnityEngine;
using TMPro;

using System.Collections.Generic;
using System;

public class TextDisplay : MonoBehaviour
{
    public enum State { Initialising, Idle, Busy }

    private TMP_Text _displayText;
    private string _displayString;
    private WaitForSeconds _shortWait;
    private WaitForSeconds _longWait;
    private State _state = State.Initialising;

    private List<string> m_DebugInput;
    private List<string> m_FunctionInput;
    private bool mb_ReadingDebugCommand;
    private bool mb_ReadingPassInNumber;
    public Game gameRef;
    public PlayerChoiceController playerChoiceController;

    public bool IsIdle { get { return _state == State.Idle; } }
    public bool IsBusy { get { return _state != State.Idle; } }

    private void Awake()
    {
        m_DebugInput = new List<string>();
        m_FunctionInput = new List<string>();
        _displayText = GetComponent<TMP_Text>();
        _shortWait = new WaitForSeconds(0.1f);
        _longWait = new WaitForSeconds(0.8f);

        _displayText.text = string.Empty;
        _state = State.Idle;
    }

    private IEnumerator DoShowText(string text)
    {
        int currentLetter = 0;
        char[] charArray = text.ToCharArray();
        string ls_DebugCommandInput = "", ls_DebugFunctionInput = "";

        while (currentLetter < charArray.Length)
        {
            if (charArray[currentLetter] == '[')
            {
                mb_ReadingDebugCommand = true;
                currentLetter++;
            }
            else if (charArray[currentLetter] == ']')
            {
                mb_ReadingDebugCommand = false;
                m_DebugInput.Add(ls_DebugCommandInput);
                currentLetter++;
                
            }
            else if (mb_ReadingDebugCommand && charArray[currentLetter] == '(')
            {
                mb_ReadingPassInNumber = true;
                currentLetter++;
            }
            else if (mb_ReadingDebugCommand && charArray[currentLetter] == ')')
            {
                mb_ReadingPassInNumber = false;
                m_FunctionInput.Add(ls_DebugFunctionInput);
                currentLetter++;
            }
            else if (mb_ReadingPassInNumber)
            {
                ls_DebugFunctionInput += charArray[currentLetter];
                currentLetter++;
                yield return _shortWait;
            }
            else if (mb_ReadingDebugCommand)
            {
                ls_DebugCommandInput += charArray[currentLetter];
                currentLetter++;
                yield return _shortWait;
            }
            else
            {
                _displayText.text += charArray[currentLetter];
                currentLetter++;
                yield return _shortWait;
            }
        }

        _displayText.text += "\n";
        _displayString = _displayText.text;
        _state = State.Idle;

        PerformTaskQueued(m_DebugInput, m_FunctionInput);
    }

    private IEnumerator DoAwaitingInput()
    {
        bool on = true;

        while (enabled)
        {
            _displayText.text = string.Format( "{0}> {1}", _displayString, ( on ? "|" : " " ));
            on = !on;
            yield return _longWait;
        }
    }

    private IEnumerator DoClearText()
    {
        int currentLetter = 0;
        char[] charArray = _displayText.text.ToCharArray();

        while (currentLetter < charArray.Length)
        {
            if (currentLetter > 0 && charArray[currentLetter - 1] != '\n')
            {
                charArray[currentLetter - 1] = ' ';
            }

            if (charArray[currentLetter] != '\n')
            {
                charArray[currentLetter] = '_';
            }

            _displayText.text = charArray.ArrayToString();
            ++currentLetter;
            yield return null;
        }

        _displayString = string.Empty;
        _displayText.text = _displayString;
        _state = State.Idle;
    }

    public void Display(string text)
    {
        if (_state == State.Idle)
        {
            StopAllCoroutines();
            _state = State.Busy;
            StartCoroutine(DoShowText(text));
        }
    }

    public void ShowWaitingForInput()
    {
        if (_state == State.Idle)
        {
            StopAllCoroutines();
            StartCoroutine(DoAwaitingInput());
        }
    }

    public void Clear()
    {
        if (_state == State.Idle)
        {
            StopAllCoroutines();
            _state = State.Busy;
            StartCoroutine(DoClearText());
        }
    }

    private void PerformTaskQueued(List<string> as_CommandRecived, List<string> as_FunctionInput)
    {
        //- To Add another command just add another option in the switch statement -//
        //- Then Perform the required Coded -//
        //- Commands are exicuted through the use of square brackets with the keyword between them- //#
        //- Eg [ESCAPE] -//
        //- This Code will be exicuted at the end of the text output-//

        int functionInputCount = 0;

        for (int i = 0; i < as_CommandRecived.Count; i++)
        {
            switch (as_CommandRecived[i])
            {
                case "ESCAPE":
                    gameRef.DisplayBeat(1);
                    break;
                case "BitCoinAdd":
                    playerChoiceController.BitCoin += (Convert.ToInt32(as_FunctionInput[functionInputCount]));
                    functionInputCount++;
                    break;
                case "BitCoinSub":
                    playerChoiceController.BitCoin -= (Convert.ToInt32(as_FunctionInput[functionInputCount]));
                    functionInputCount++;
                    break;
                case "CarmaAdd":
                    playerChoiceController.CarmaAmount += (Convert.ToInt32(as_FunctionInput[functionInputCount]));
                    functionInputCount++;
                    break;
                case "CarmaSub":
                    playerChoiceController.CarmaAmount -= (Convert.ToInt32(as_FunctionInput[functionInputCount]));
                    functionInputCount++;
                    break;
                default:
                    Debug.LogError("COMMAND NOT FOUND PLEASE ADD IT TO PerformTaskQueued() or Remove The Use of []");
                    break;
            }
        }

        as_CommandRecived.Clear();
        as_FunctionInput.Clear();
    }
}
