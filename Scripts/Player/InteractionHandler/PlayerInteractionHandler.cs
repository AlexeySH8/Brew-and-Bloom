using UnityEngine;

public class PlayerInteractionHandler : MonoBehaviour
{
    private InteractionDetector _detector;
    private PlayerItemHolder _itemHolder;

    private void Awake()
    {
        _detector = GetComponent<InteractionDetector>();
        _itemHolder = GetComponent<PlayerItemHolder>();
    }

    public void Interact(float faceDirection)
    {
        GameObject heldItem = _itemHolder.GetHeldItem();
        RaycastHit2D hit = _detector.DetectInterectiveItem(heldItem != null);
        Collider2D interactiveItem = hit.collider;

        if (heldItem != null)
        {
            if (TryUseItem(heldItem)) return;

            if (interactiveItem == null) return;

            HandleFullHandInteraction(heldItem, interactiveItem);
            return;
        }

        if (interactiveItem != null)
            HandleEmptyHandInteraction(interactiveItem);
    }

    private void HandleFullHandInteraction(GameObject heldItem, Collider2D target)
    {
        if (target.TryGetComponent(out IReceiveHeldItem receiver))
        {
            receiver.TryReceive(heldItem);
        }
        else if (target.TryGetComponent(out IFreeInteractable interactable))
        {
            interactable.Interact();
        }
        return;
    }

    private void HandleEmptyHandInteraction(Collider2D target)
    {
        if (target.TryGetComponent(out BaseHoldItem holdItem))
        {
            _itemHolder.PickUp(holdItem);
        }
        else if (target.TryGetComponent(out IGiveHeldItem giver))
        {
            BaseHoldItem item = giver.Give().GetComponent<BaseHoldItem>();
            _itemHolder.PickUp(item);
        }
        else if (target.TryGetComponent(out IFreeInteractable interactable))
        {
            interactable.Interact();
        }
    }

    private bool TryUseItem(GameObject heldItem)
    {
        if (heldItem.TryGetComponent(out BaseTool tool))
        {
            var toolTarget = _detector
                .DetectToolTarget(tool.InteractionDistance, tool.InteractionMask);
            if (toolTarget)
            {
                tool.Use(toolTarget);
                return true;
            }
        }
        return false;
    }
}
