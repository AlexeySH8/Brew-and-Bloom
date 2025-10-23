using System.Collections.Generic;
using UnityEngine;

public class ItemPool : MonoBehaviour
{
    public static ItemPool Instance { get; private set; }

    private const int MaxItemCountInScene = 150;
    private Queue<BaseHoldItem> _queueHoldItem = new Queue<BaseHoldItem>();
    private HashSet<BaseHoldItem> _hashSetHoldItems = new HashSet<BaseHoldItem>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Duplicate ItemPool detected, destroying new one.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Register(BaseHoldItem newItem)
    {
        if (_hashSetHoldItems.Contains(newItem)) return;

        if (_hashSetHoldItems.Count >= MaxItemCountInScene)
            DiscardOldestItem();

        _queueHoldItem.Enqueue(newItem);
        _hashSetHoldItems.Add(newItem);
    }

    public void Unregister(BaseHoldItem item)
    {
        _hashSetHoldItems.Remove(item);
    }

    private void DiscardOldestItem()
    {
        _queueHoldItem
            .Dequeue()
            .Discard();
    }
}
