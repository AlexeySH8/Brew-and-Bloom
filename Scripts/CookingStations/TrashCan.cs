using UnityEngine;

public class TrashCan : MonoBehaviour, ICookingStation
{
    public void Cook(GameObject ingredient)
    {
        if (ingredient.TryGetComponent(out IHoldItem holdItem))
            holdItem.Discard();
    }
}
