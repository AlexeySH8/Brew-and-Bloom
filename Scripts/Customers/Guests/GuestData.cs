using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "NewGuestData", menuName = "Farming/Guest Data")]
public class GuestData : ScriptableObject
{
    [field: SerializeField] public LocalizedString Name { get; private set; }
    [field: SerializeField] public string GuestId { get; private set; }
    [field: SerializeField] public int MinPayment { get; private set; }
    [field: SerializeField] public int MaxPayment { get; private set; }
    [field: SerializeField] public GameObject GuestPrefab { get; private set; }
    [field: SerializeField] public Sprite Portrait { get; private set; }
    [field: SerializeField] public DialogueData DefaultDialogueData { get; private set; }
    [field: SerializeField] public AudioClip TypingSound { get; private set; }
    [field: SerializeField] public DialogueData[] DialoguesData { get; private set; }

#if UNITY_EDITOR
    [ContextMenu("Generate Unique Id")]
    private void GenerateUniqueId()
    {
        GuestId = GUID.Generate().ToString();
        EditorUtility.SetDirty(this);
    }
#endif
}
