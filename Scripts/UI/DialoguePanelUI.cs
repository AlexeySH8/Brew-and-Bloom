using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class DialoguePanelUI : MonoBehaviour
{
    [SerializeField] private GameObject _portretBox;
    [SerializeField] private GameObject _dialogueBox;
    [SerializeField] private Image _guestPortret;
    [SerializeField] private TextMeshProUGUI _guestName;
    [SerializeField] private TextMeshProUGUI _dialogueText;

    private Coroutine _typing;

    public void StarDialogue(Sprite guestPortrait, string guestName)
    {
        _guestPortret.sprite = guestPortrait;
        _guestName.text = guestName;
        _dialogueText.SetText("");

        _portretBox.GetComponent<SlideAnimation>().Transition(true);
        _dialogueBox.GetComponent<SlideAnimation>().Transition(true);
    }

    public void TypeLine(string line, float typingSpeed)
    {
        if (_typing != null)
        {
            StopCoroutine(_typing);
            _typing = null;
        }
        _typing = StartCoroutine(TypeLineRoutine(line, typingSpeed));
    }

    public IEnumerator TypeLineRoutine(string line, float typingSpeed)
    {
        _dialogueText.SetText("");
        foreach (char letters in line)
        {
            _dialogueText.text += letters;
            yield return new WaitForSeconds(typingSpeed);
        }
        _typing = null;
    }

    public void EndDialogue()
    {
        _portretBox.GetComponent<SlideAnimation>().Transition(false);
        _dialogueBox.GetComponent<SlideAnimation>().Transition(false);
    }
}
