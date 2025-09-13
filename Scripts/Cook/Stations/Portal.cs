using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour, IReceiveHeldItem
{
    [SerializeField] private OrdersManager _ordersManager;

    public void Receive(GameObject heldItem)
    {
        if (heldItem.TryGetComponent(out Dish dish))
        {
            _ordersManager.AddPlayerDish(dish.Data);
            dish.GetComponent<BaseHoldItem>().Discard();
        }
    }
}
