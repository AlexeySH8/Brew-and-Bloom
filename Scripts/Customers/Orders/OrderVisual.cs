using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OrderVisual : MonoBehaviour
{
    [SerializeField] private OrdersUI _ordersUI;

    private Dictionary<Guest, OrderTemplate> _activeOrders = new Dictionary<Guest, OrderTemplate>();
    private Queue<Order> _pendingOrders = new Queue<Order>();
    private List<OrderTemplate> _availableOrderTemplates;

    private void Start()
    {
        _availableOrderTemplates = new List<OrderTemplate>(_ordersUI.OrderTemplates);
    }

    public void AddOrder(Order order)
    {
        OrderTemplate orderTemplate = _availableOrderTemplates.FirstOrDefault();
        if (orderTemplate != null)
        {
            _availableOrderTemplates.Remove(orderTemplate);
            _activeOrders.Add(order.Guest, orderTemplate);
            orderTemplate.DisplayOrder(order.Dish);
            return;
        }
        _pendingOrders.Enqueue(order);
    }

    public void RemoveOrder(Order order)
    {
        OrderTemplate orderTemplate = _activeOrders[order.Guest];
        orderTemplate.ClearVisual();
        _activeOrders.Remove(order.Guest);

        if (_pendingOrders.Count == 0)
        {
            _availableOrderTemplates.Add(orderTemplate);
        }
        else
        {
            Order pendingOrder = _pendingOrders.Dequeue();
            orderTemplate.DisplayOrder(pendingOrder.Dish);
            _activeOrders.Add(pendingOrder.Guest, orderTemplate);
        }
    }
}
