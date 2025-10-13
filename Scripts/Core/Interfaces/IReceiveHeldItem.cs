using UnityEngine;

public interface IReceiveHeldItem
{
    bool TryReceive(GameObject heldItem);
}
