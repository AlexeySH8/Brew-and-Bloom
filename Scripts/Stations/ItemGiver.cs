using UnityEngine;
using Zenject;

public class ItemGiver : MonoBehaviour, IGiveHeldItem
{
    [SerializeField] private BaseHoldItem _itemPrefab;

    public BaseHoldItem GiveItem()
    {
        return Instantiate(_itemPrefab);
    }

    public bool HasItem() => true;
}
