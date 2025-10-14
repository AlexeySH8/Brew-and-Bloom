using UnityEngine;

public class TrashCan : MonoBehaviour, IReceiveHeldItem
{
    public bool TryReceive(BaseHoldItem heldItem)
    {
        Utilize(heldItem);
        return true;
    }

    public void Utilize(BaseHoldItem holdItem) => holdItem.Discard();
}
