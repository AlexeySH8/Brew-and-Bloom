using UnityEngine;

public interface IItemHolder
{
    Transform ParentPoint { get; }
    int SortingOrderOffset { get; }
    void OnItemRemoved(BaseHoldItem holdItem);
}
