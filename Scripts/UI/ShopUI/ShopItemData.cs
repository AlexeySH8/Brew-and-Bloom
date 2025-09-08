using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopItemData", menuName = "Farming/ShopItem Data")]
public class ShopItemData : ScriptableObject
{
    [field: SerializeField] public GameObject Item { get; private set; }
    [field: SerializeField] public int Price { get; private set; }
}
