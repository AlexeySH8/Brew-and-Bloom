using System.Linq;
using UnityEngine;
using Zenject;

public abstract class BaseItemHolder : MonoBehaviour, IGiveHeldItem, IReceiveHeldItem
{
    public abstract Transform ParentPoint { get; }
    public abstract int SortingOrderOffset { get; }
    public BaseHoldItem HeldItem => _heldItem;
    protected BaseHoldItem _heldItem;
    
    protected virtual void Awake()
    {
        CheckChildren();
    }

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

    private void CheckChildren()
    {
        if (ParentPoint.childCount == 0) return;

        for (int i = 0; i < ParentPoint.childCount; i++)
        {
            Transform child = ParentPoint.GetChild(i);

            // If the object was manually inserted into the parent
            if (child.TryGetComponent(out BaseHoldItem holdItem) &&
                !holdItem.HasParent())
            {
                if (!TryReceive(holdItem))
                    Debug.LogError($"Object {holdItem.name} cannot be inserted into parent {this.name}");
            }
        }
    }
}
