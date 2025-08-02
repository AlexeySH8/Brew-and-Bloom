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

    public void Interact(float faceDirection)
    {
        var interactiveItem = _detector.Detect();

        if (_itemHolder.GetHeldItem())
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
                tool.Use(faceDirection);
                return;
            }
        }
        else if (interactiveItem.collider != null && interactiveItem.collider.TryGetComponent(out BaseItemDispenser pickupShelf))
        {
            _itemHolder.PickUp(pickupShelf.DispenseItem());
        }
        else if (interactiveItem.rigidbody != null && interactiveItem.rigidbody.TryGetComponent(out IHoldItem holdItem))
        {
            _itemHolder.PickUp(interactiveItem.rigidbody.gameObject);
            return;
        }
    }
}
