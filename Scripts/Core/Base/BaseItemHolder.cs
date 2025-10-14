using UnityEngine;

public abstract class BaseItemHolder : MonoBehaviour, IGiveHeldItem, IReceiveHeldItem
{
    public abstract Transform ParentPoint { get; }
    public abstract int SortingOrderOffset { get; }

    protected BaseHoldItem _heldItem;

    public BaseHoldItem HeldItem => _heldItem;

    public virtual bool TryReceive(BaseHoldItem heldItem)
    {
        if (_heldItem != null) return false;

        if (IsCorrectItemToReceive(heldItem))
        {
            _heldItem = heldItem;
            _heldItem.SetHolder(this);
            return true;
        }
        return false;
    }

    public virtual BaseHoldItem GiveItem()
    {
        if (_heldItem == null) return null;

        BaseHoldItem item = _heldItem;
        _heldItem.SetHolder(null);
        _heldItem = null;
        return item;
    }

    protected virtual bool IsCorrectItemToReceive(BaseHoldItem heldItem) => true;

    public virtual void OnItemRemoved(BaseHoldItem holdItem)
    {
        if (_heldItem == null || _heldItem != holdItem) return;
        _heldItem = null;
    }
}
