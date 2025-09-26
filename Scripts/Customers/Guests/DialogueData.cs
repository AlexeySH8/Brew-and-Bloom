using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "NewDialogueDataData", menuName = "Farming/DialogueData Data")]
public class DialogueData : ScriptableObject
{
    public float TypingSpeed { get; private set; } = 0.05f;
    public float AutoProgresselay { get; private set; } = 1.5f;
    [field: SerializeField] public LocalizedString[] DialogueLines { get; private set; }
    [field: SerializeField] public bool[] AutoProgressLines { get; private set; }
}
