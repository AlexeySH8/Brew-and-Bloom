using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerData", menuName = "Farming/Player Data")]
public class PlayerData : ScriptableObject
{
    [field: SerializeField] public int Balance { get; set; }
    [field: SerializeField] public int DailyEarning { get; set; }
}
