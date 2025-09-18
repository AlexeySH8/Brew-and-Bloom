using UnityEngine;

[CreateAssetMenu(fileName = "NewGuestData", menuName = "Farming/Guest Data")]
public class GuestData : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public int MinPayment { get; private set; }
    [field: SerializeField] public int MaxPayment { get; private set; }
    [field: SerializeField] public GameObject GuestPrefab { get; private set; }
    [field: SerializeField] public Sprite Portrait { get; private set; }
}
