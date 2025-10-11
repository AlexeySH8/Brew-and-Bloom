using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfPoint : MonoBehaviour, IItemHolder, IGiveHeldItem, IReceiveHeldItem
{
    [SerializeField] private LayerMask _toolInteractionLayer;

    private BaseHoldItem _tool;

    public Transform ParentPoint => transform;

    public int SortingOrderOffset =>
        transform.parent.GetComponent<SpriteRenderer>().sortingOrder + 1;

    private void Start()
    {
        CheckChildren();
    }

    public GameObject Give()
    {
        if (_tool == null) return null;

        GameObject tool = _tool.gameObject;
        _tool.SetHolder(null);
        _tool = null;
        return tool;
    }

    public bool TryReceive(GameObject heldItem)
    {
        if (_tool != null) return false;

        if (IsCorrectItem(heldItem))
        {
            _tool = heldItem.GetComponent<BaseHoldItem>();
            _tool.SetHolder(this);
            return true;
        }
        return false;
    }

    private bool IsCorrectItem(GameObject holdItem) //only a specific item can be placed on the shelf
    {
        return holdItem.TryGetComponent(out BaseUsableItem usableItem) &&
            usableItem.InteractionMask == _toolInteractionLayer;
    }

    public void OnItemRemoved(BaseHoldItem holdItem)
    {
        _tool = null;
    }

    private void CheckChildren()
    {
        Transform child = transform.GetChild(0);
        if (child == null) return;

        if (!TryReceive(child.gameObject))
            Debug.LogError($"The wrong {child.name} of the parent {this.name}");
    }
}
