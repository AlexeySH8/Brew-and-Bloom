using UnityEngine;

public class ItemStand : BaseItemHolder
{
    [SerializeField] private Transform _standPoint;

    private SpriteRenderer _itemStandVisual;

    public override Transform ParentPoint => _standPoint;
    public override int SortingOrderOffset => _itemStandVisual.sortingOrder + 1;

    private void Awake()
    {
        _itemStandVisual = GetComponent<SpriteRenderer>();
    }
}
