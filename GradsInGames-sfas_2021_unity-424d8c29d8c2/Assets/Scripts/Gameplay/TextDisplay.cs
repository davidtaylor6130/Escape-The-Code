using System.Collections;
using UnityEngine;
using TMPro;

using System.Collections.Generic;

public class TextDisplay : MonoBehaviour
{
    public enum State { Initialising, Idle, Busy }

    private TMP_Text _displayText;
    private string _displayString;
    private WaitForSeconds _shortWait;
    private WaitForSeconds _longWait;
    private State _state = State.Initialising;

    private List<string> m_DebugInput;
    private bool mb_ReadingDebugCommand;
    public Game gameRef;

    public bool IsIdle { get { return _state == State.Idle; } }
    public bool IsBusy { get { return _state != State.Idle; } }

    private void Awake()
    {
        m_DebugInput = new List<string>();
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
        string ls_DebugInput = "";

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
                m_DebugInput.Add(ls_DebugInput);
                currentLetter++;
                
            }
            else if (mb_ReadingDebugCommand)
            {
                ls_DebugInput += charArray[currentLetter];
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

        PerformTaskQueued(m_DebugInput);
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

    private void PerformTaskQueued(List<string> as_CommandRecived)
    {
        //- To Add another command just add another option in the switch statement -//
        //- Then Perform the required Coded -//
        //- Commands are exicuted through the use of square brackets with the keyword between them- //#
        //- Eg [ESCAPE] -//
        //- This Code will be exicuted at the end of the text output-//

        for (int i = 0; i < as_CommandRecived.Count; i++)
        {
            switch (as_CommandRecived[i])
            {
                case "ESCAPE":
                    gameRef.DisplayBeat(1);
                    break;
                default:
                    Debug.LogError("COMMAND NOT FOUND PLEASE ADD IT TO PerformTaskQueued() or Remove The Use of []");
                    break;
            }
        }

        as_CommandRecived.Clear();
    }
}
