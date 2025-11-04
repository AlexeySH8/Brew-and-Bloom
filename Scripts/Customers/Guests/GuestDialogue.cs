using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;

public class GuestDialogue : IActiveInteraction
{
    private int _lineIndex;
    private DialogueData[] _dialoguesData;
    private DialogueData _defaultDialogueData;
    private DialogueData _dialogueDataPart;
    private PlayerController _playerController;
    private DialoguePanelUI _dialoguePanelUI;
    private Sprite _guestPortret;
    private string _guestName;
    private AudioClip _guestTypingSound;
    private LocalizedString _currentLine;
    private bool _isStoryFinished;
    private static readonly string DEFAULT_DIALOGUE_PATH = "Assets/Data/Dialogue/DefaultDialogue.asset";

    public GuestDialogue(GuestData guestData)
    {
        _dialoguesData = guestData.DialoguesData;
        _guestPortret = guestData.Portrait;
        _guestName = guestData.Name;
        _guestTypingSound = guestData.TypingSound;
        _isStoryFinished = false;

        if (guestData.DefaultDialogueData == null)
        {
            _defaultDialogueData = Addressables
                .LoadAssetAsync<DialogueData>(DEFAULT_DIALOGUE_PATH)
                .WaitForCompletion();
        }
        else
            _defaultDialogueData = guestData.DefaultDialogueData;

    }

    public void HandleInteractPressed() => NextLineDialogue();

    public void EndActiveInteraction() => EndDialogue();

    public void StartDialogue(int dialoguePartIndex)
    {
        #region Exception Check
        if (_playerController == null)
        {
            _playerController = GameObject.FindAnyObjectByType<PlayerController>();
            if (_playerController == null)
            {
                Debug.LogError("Player not found");
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
        SetDialoguePart(dialoguePartIndex);
        _dialoguePanelUI.StartDialogue(_guestPortret, _guestName);
        _playerController.StartActiveInteraction(this);
        NextLineDialogue();
    }

    private void NextLineDialogue()
    {
        if (_dialoguePanelUI.IsTyping()) return;

        if (_currentLine != null)
            _currentLine.StringChanged -= OnStringChanged;

        if (!_isStoryFinished && _lineIndex < _dialogueDataPart.DialogueLines.Length)
        {
            _currentLine = _dialogueDataPart.DialogueLines[_lineIndex];

            _currentLine.StringChanged += OnStringChanged; //Call TypeLine
            _lineIndex++;
        }
        else if (_isStoryFinished && _lineIndex == 0)
        {
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

    private void SetDialoguePart(int dialoguePartIndex)
    {
        if (_isStoryFinished) return;

        if (_dialoguesData.Length == 0 ||
            dialoguePartIndex >= _dialoguesData.Length)
        {
            _isStoryFinished = true;
            _dialogueDataPart = _defaultDialogueData;
            return;
        }
        _dialogueDataPart = _dialoguesData[dialoguePartIndex];
    }

    private void EndDialogue()
    {
        if (_currentLine != null)
            _currentLine.StringChanged -= OnStringChanged;

        _playerController.EndActiveInteraction();
        _dialoguePanelUI.EndDialogue();
    }

    // TypeLine
    private void OnStringChanged(string newValue)
    {
        _dialoguePanelUI.TypeLine(_currentLine.GetLocalizedString(),
            _dialogueDataPart.TypingSpeed, _guestTypingSound);
    }
}