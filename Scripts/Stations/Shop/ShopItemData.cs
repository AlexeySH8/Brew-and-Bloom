using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopItemData", menuName = "Farming/ShopItem Data")]
public class ShopItemData : ScriptableObject
{
    [field: SerializeField] public int Price { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public GameObject ItemPrefab { get; private set; }
    [field: SerializeField] public ShopServiceType ServiceType { get; private set; }
}

public enum ShopServiceType
{
    None,
    AddBut,
    EndGame
}
