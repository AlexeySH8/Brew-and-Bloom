using UnityEngine;

public class TrashCan : MonoBehaviour, IReceiveHeldItem
{
    public void Receive(GameObject heldItem) => Utilize(heldItem);

    public void Utilize(GameObject item)
    {
        if (item.TryGetComponent(out BaseHoldItem holdItem))
            holdItem.Discard();
    }
}
