using UnityEngine;

public class ShelfPoint : BaseItemHolder
{
    [SerializeField] private LayerMask _toolInteractionLayer;

    public override Transform ParentPoint => transform;

    public override int SortingOrderOffset =>
        transform.parent.GetComponent<SpriteRenderer>().sortingOrder + 1;

    private void Start()
    {
        CheckChildren();
    }

    protected override bool IsCorrectItemToReceive(BaseHoldItem heldItem)
    {
        return heldItem.TryGetComponent(out BaseUsableItem usableItem) &&
            usableItem.InteractionMask == _toolInteractionLayer;
    }

    private void CheckChildren()
    {
        Transform child = transform.GetChild(0);
        if (child == null) return;

        if (!TryReceive(child.GetComponent<BaseHoldItem>()))
            Debug.LogError($"The wrong {child.name} of the parent {name}");
    }
}
