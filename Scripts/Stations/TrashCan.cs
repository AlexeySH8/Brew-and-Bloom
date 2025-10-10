using UnityEngine;

public class TrashCan : MonoBehaviour, IReceiveHeldItem
{
    public bool TryReceive(GameObject heldItem)
    {
        return Utilize(heldItem);
    }

    public bool Utilize(GameObject item)
    {
        if (item.TryGetComponent(out BaseHoldItem holdItem))
        {
            holdItem.Discard();
            return true;
        }
        return false;
    }
}
