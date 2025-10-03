using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "NewDialogueDataData", menuName = "Farming/DialogueData Data")]
public class DialogueData : ScriptableObject
{
    [field: SerializeField] public float TypingSpeed { get; private set; } = 0.03f;
    [field: SerializeField] public LocalizedString[] DialogueLines { get; private set; }
}
