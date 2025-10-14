using UnityEngine;

public class ItemGiver : MonoBehaviour , IGiveHeldItem
{
    [SerializeField] private BaseHoldItem _itemPrefab;

    public BaseHoldItem GiveItem()
    {
        return Instantiate(_itemPrefab);
    }
}
