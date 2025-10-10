using UnityEngine;

public interface IItemHolder
{
    Transform ParentPoint { get; }
    int SortingOrderOffset { get; }
    void ItemReceived(BaseHoldItem holdItem);
    void ItemRemoved(BaseHoldItem holdItem);
}
