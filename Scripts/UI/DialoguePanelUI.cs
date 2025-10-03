using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialoguePanelUI : MonoBehaviour
{
    [SerializeField] private GameObject _portretBox;
    [SerializeField] private GameObject _dialogueBox;
    [SerializeField] private Image _guestPortret;
    [SerializeField] private TextMeshProUGUI _guestName;
    [SerializeField] private TextMeshProUGUI _dialogueText;


    private char[] _sentenceDelimiters = new char[]
{
    '.',
    '!',
    '?',
    ';',
    ':',
    '…'
};
    private Coroutine _typing;

    public void StartDialogue(Sprite guestPortrait, string guestName)
    {
        _guestPortret.sprite = guestPortrait;
        _guestName.text = guestName;
        _dialogueText.SetText("");

        _portretBox.GetComponent<SlideAnimation>().Transition(true);
        _dialogueBox.GetComponent<SlideAnimation>().Transition(true);
    }

    public void TypeLine(string line, float typingSpeed, AudioClip typingSound)
    {
        if (_typing != null)
        {
            StopCoroutine(_typing);
            _typing = null;
        }
        _typing = StartCoroutine(TypeLineRoutine(line, typingSpeed, typingSound));
    }

    public IEnumerator TypeLineRoutine(string line, float typingSpeed, AudioClip typingSound)
    {
        _dialogueText.SetText("");
        int counter = 0;

        foreach (char letters in line)
        {
            _dialogueText.text += letters;
            if (counter % 2 == 0)
                SFX.Instance.PlayAudioClip(typingSound);

            if (_sentenceDelimiters.Contains(letters))
            {
                yield return new WaitForSeconds(Random.Range(0.3f, 0.5f));
            }

            counter++;
            yield return new WaitForSeconds(typingSpeed);
        }
        _typing = null;
    }

    public void EndDialogue()
    {
        _portretBox.GetComponent<SlideAnimation>().Transition(false);
        _dialogueBox.GetComponent<SlideAnimation>().Transition(false);
    }

    public bool IsTyping() => _typing != null;
}
