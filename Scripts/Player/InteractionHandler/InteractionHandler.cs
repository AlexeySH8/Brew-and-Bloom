using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
    private InteractionDetector _detector;
    private ItemHolder _itemHolder;

    private void Awake()
    {
        _detector = GetComponent<InteractionDetector>();
        _itemHolder = GetComponent<ItemHolder>();
    }

    public void Interact()
    {
        var interactiveItem = _detector.Detect();

        if (_itemHolder.HasItem)
        {
            var heldItem = _itemHolder.GetHeldItem();
            if (interactiveItem.collider != null && interactiveItem.collider.TryGetComponent(out ICookingStation cookingStation))
            {
                cookingStation.Cook(heldItem);
                _itemHolder.Clear();
                return;
            }
            else if (heldItem.TryGetComponent(out BaseTool tool))
            {
                tool.Use();
                return;
            }
        }
        else if (interactiveItem.rigidbody != null && interactiveItem.rigidbody.TryGetComponent(out IHoldItem holdItem))
        {
            _itemHolder.PickUp(interactiveItem.rigidbody.gameObject);
            return;
        }
    }
}
