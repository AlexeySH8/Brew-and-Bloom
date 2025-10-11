using UnityEngine;

public class ItemStand : MonoBehaviour, IGiveHeldItem, IReceiveHeldItem, IItemHolder
{
    [SerializeField] private Transform _standPoint;

    private SpriteRenderer _itemStandVisual;
    private BaseHoldItem _standingItem;

    public Transform ParentPoint => _standPoint;

    public int SortingOrderOffset => _itemStandVisual.sortingOrder + 1;

    private void Awake()
    {
        _itemStandVisual = GetComponent<SpriteRenderer>();
    }

    public GameObject Give()
    {
        if (_standingItem == null) return null;

        GameObject item = _standingItem.gameObject;
        _standingItem.SetHolder(null);
        _standingItem = null;
        return item;
    }

    public bool TryReceive(GameObject heldItem)
    {
        if (_standingItem != null) return false;

        _standingItem = heldItem.GetComponent<BaseHoldItem>();
        _standingItem.SetHolder(this);

        return true;
    }

    public void OnItemRemoved(BaseHoldItem holdItem)
    {
        _standingItem = null;
    }
}
