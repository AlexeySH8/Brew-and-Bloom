using UnityEngine;

public class BaseItemDispenser : MonoBehaviour
{
    [SerializeField] private GameObject _itemPrefab;

    public GameObject DispenseItem()
    {
        return Instantiate(_itemPrefab);
    }
}
