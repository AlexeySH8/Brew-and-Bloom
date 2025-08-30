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

        if (TryUseTool(heldItem)) return;

        if (interactiveItem.collider == null) return;

        if (heldItem && interactiveItem.collider.TryGetComponent(out IReceiveHeldItem receiver))
        {
            receiver.Receive(heldItem);
        }
        else if (interactiveItem.collider.TryGetComponent(out IGiveHeldItem giver))
        {
            _itemHolder.PickUp(giver.Give());
        }
        else if (interactiveItem.collider.TryGetComponent(out IFreeInteractable interactable))
        {
            interactable.Interact();
        }
    }

    private bool TryUseTool(GameObject heldItem)
    {
        if (heldItem && heldItem.TryGetComponent(out BaseTool tool))
        {
            var toolTarget = _detector.DetectToolTarget(tool.InteractionDistance, tool.InteractionMask);
            if (toolTarget)
            {
                tool.Use(toolTarget);
                return true;
            }
        }
        return false;
    }
}
