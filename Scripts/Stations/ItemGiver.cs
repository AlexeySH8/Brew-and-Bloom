using UnityEngine;

public class ItemGiver : MonoBehaviour , IGiveHeldItem
{
    [SerializeField] private GameObject _itemPrefab;

    public GameObject Give()
    {
        return Instantiate(_itemPrefab);
    }
}
