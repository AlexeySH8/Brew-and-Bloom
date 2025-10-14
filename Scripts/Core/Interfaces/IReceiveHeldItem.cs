using UnityEngine;

public interface IReceiveHeldItem
{
    bool TryReceive(BaseHoldItem heldItem);
}
