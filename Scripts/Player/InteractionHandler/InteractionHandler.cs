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
        var interactiveItem = _detector.DetectInterectiveItem();
        var heldItem = _itemHolder.GetHeldItem();

        if (heldItem)
        {
            if (interactiveItem.collider != null && interactiveItem.collider.TryGetComponent(out IReceivesHeldItem receiver))
            {
                receiver.Receive(heldItem);
            }
            else if (heldItem.TryGetComponent(out BaseTool tool))
            {
                var toolTarget = _detector.DetectToolTrget(tool.InteractionDistance, tool.InteractionMask);
                if (toolTarget)
                    tool.Use(toolTarget);
            }
            return;
        }
        else if (interactiveItem.collider != null && interactiveItem.collider.TryGetComponent(out BaseItemDispenser dispenser))
        {
            _itemHolder.PickUp(dispenser.DispenseItem());
        }
        else if (interactiveItem.rigidbody != null && interactiveItem.rigidbody.TryGetComponent(out IHoldItem holdItem))
        {
            _itemHolder.PickUp(interactiveItem.rigidbody.gameObject);
            return;
        }
    }
}
