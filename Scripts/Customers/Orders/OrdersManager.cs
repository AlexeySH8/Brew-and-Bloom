using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrdersManager : MonoBehaviour
{
    [SerializeField] private List<Order> _completedOrders = new List<Order>();
    [SerializeField] private List<Order> _orders = new List<Order>();

    private OrderVisual _orderVisual;

    private void Awake()
    {
        _orderVisual = GetComponent<OrderVisual>();
    }

    private void Update()
    {
        foreach (Order order in _orders)
        {
            if (order.IsCompleted && !_completedOrders.Contains(order))
                OrderCompleted(order);
        }
    }

    private void Start()
    {
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        GuestsManager.Instance.OnGuestArrived += AcceptOrders;
    }

    private void OnDisable()
    {
        GuestsManager.Instance.OnGuestArrived -= AcceptOrders;
    }

    private void AcceptOrders(Guest guest)
    {
        _orders.Add(guest.CurrentOrder);
        _orderVisual.AddOrder(guest.CurrentOrder);
    }

    private void OrderCompleted(Order order)
    {
        _completedOrders.Add(order);
        _orderVisual.RemoveOrder(order);
    }
}
