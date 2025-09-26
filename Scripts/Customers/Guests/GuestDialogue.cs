using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GuestDialogue
{
    private int _dialoguePartIndex;
    private int _lineIndex;
    private DialogueData[] _dialoguesData;
    private DialogueData _dialogueDataPart;
    private PlayerController _playerController;
    private DialoguePanelUI _dialoguePanelUI;
    private Sprite _guestPortret;
    private string _guestName;
    private LocalizedString _currentLine;
    private bool _isStoryFinished;
    private static readonly string DEFAULT_DIALOGUE_PATH = "Assets/Data/Dialogue/DefaultDialogue.asset";

    public GuestDialogue(DialogueData[] dialoguesData, Sprite guestPortret, string guestName)
    {
        _dialoguesData = dialoguesData;
        _guestPortret = guestPortret;
        _guestName = guestName;
        _dialoguePartIndex = 0;
        _isStoryFinished = false;
    }

    public void StartDialogue()
    {
        #region Exception Check
        if (_dialoguesData.Length == 0)
        {
            Debug.LogError("The dialog array is empty");
            return;
        }

        if (_playerController == null)
        {
            _playerController = GameObject.FindAnyObjectByType<PlayerController>();
            if (_playerController == null)
            {
                Debug.LogError("DialoguePanel not found");
                return;
            }
        }

        if (_dialoguePanelUI == null)
        {
            _dialoguePanelUI = GameObject.FindAnyObjectByType<DialoguePanelUI>();
            if (_playerController == null)
            {
                Debug.LogError("DialoguePanel not found");
                return;
            }
        }
        #endregion

        _lineIndex = 0;
        SetDialoguePart();
        _dialoguePanelUI.StarDialogue(_guestPortret, _guestName);
        _playerController.StartDialogue(this);
        NextLineDialogue();
    }

    public void NextLineDialogue()
    {
        if (!_isStoryFinished && _lineIndex < _dialogueDataPart.DialogueLines.Length)
        {
            if (_currentLine != null)
                _currentLine.StringChanged -= OnStringChanged;

            _currentLine = _dialogueDataPart.DialogueLines[_lineIndex];

            _currentLine.StringChanged += OnStringChanged; //Call TypeLine
            _lineIndex++;
        }
        else if (_isStoryFinished && _lineIndex == 0)
        {
            if (_currentLine != null)
                _currentLine.StringChanged -= OnStringChanged;

            // Select random lines from Default 
            _currentLine = _dialogueDataPart.DialogueLines[Random.Range(
                    0, _dialogueDataPart.DialogueLines.Length)];

            //Call TypeLine
            _currentLine.StringChanged += OnStringChanged;
            _lineIndex++;
        }
        else
            EndDialogue();
    }

    private void SetDialoguePart()
    {
        if (_isStoryFinished) return;

        if (_dialoguePartIndex == _dialoguesData.Length)
        {
            _isStoryFinished = true;
            _dialogueDataPart = Addressables
                .LoadAssetAsync<DialogueData>(DEFAULT_DIALOGUE_PATH)
                .WaitForCompletion();
            return;
        }
        _dialogueDataPart = _dialoguesData[_dialoguePartIndex];
    }

    private void EndDialogue()
    {
        _playerController.EndDialogue();
        _dialoguePanelUI.EndDialogue();
    }

    public void SetNextDialoguePart() => _dialoguePartIndex++;

    private void OnStringChanged(string newValue)
    {
        _dialoguePanelUI.TypeLine(_currentLine.GetLocalizedString(), _dialogueDataPart.TypingSpeed);
    }
}
