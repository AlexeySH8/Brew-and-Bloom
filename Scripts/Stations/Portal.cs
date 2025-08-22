using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour, IReceivesHeldItem
{
    public void Receive(GameObject heldItem)
    {
        if (heldItem.TryGetComponent(out Dish dish))
        {
            dish.GetComponent<BaseHoldItem>().Discard();
        }
    }
}
