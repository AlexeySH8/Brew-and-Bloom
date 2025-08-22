using System.Linq;
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
        GameObject heldItem = _itemHolder.GetHeldItem();
        RaycastHit2D interactiveItem = _detector.DetectInterectiveItem(heldItem != null);

        if (heldItem)
        {
            if (interactiveItem.collider != null && 
                interactiveItem.collider.TryGetComponent(out IReceivesHeldItem receiver))
            {
                receiver.Receive(heldItem);
            }
            else if (heldItem.TryGetComponent(out BaseTool tool))
            {
                var toolTarget = _detector.DetectToolTarget(tool.InteractionDistance, tool.InteractionMask);
                if (toolTarget)
                    tool.Use(toolTarget);
            }
        }
        else if (interactiveItem.collider != null && 
            interactiveItem.collider.TryGetComponent(out BaseItemDispenser dispenser))
        {
            _itemHolder.PickUp(dispenser.DispenseItem());
        }
        else if (interactiveItem.rigidbody != null && 
            interactiveItem.rigidbody.TryGetComponent(out BaseHoldItem holdItem))
        {
            _itemHolder.PickUp(interactiveItem.rigidbody.gameObject);
        }
    }
}
