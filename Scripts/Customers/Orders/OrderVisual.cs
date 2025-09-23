using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class OrderVisual : MonoBehaviour
{
    public List<OrderTemplate> OrderTemplates { get; private set; }

    [SerializeField] private TextMeshProUGUI _ordersCountText;

    private Dictionary<Guest, OrderTemplate> _activeOrders = new Dictionary<Guest, OrderTemplate>();
    private Queue<Order> _pendingOrders = new Queue<Order>();
    private List<OrderTemplate> _availableOrderTemplates;

    private void Awake()
    {
        OrderTemplates = FindObjectsOfType<OrderTemplate>().ToList();
        _availableOrderTemplates = new List<OrderTemplate>(OrderTemplates);
    }

    public void AddOrder(Order order)
    {
        OrderTemplate orderTemplate = _availableOrderTemplates.FirstOrDefault();
        if (orderTemplate != null)
        {
            _availableOrderTemplates.Remove(orderTemplate);
            _activeOrders.Add(order.Guest, orderTemplate);
            orderTemplate.DisplayOrder(order.Dish);
        }
        else
            _pendingOrders.Enqueue(order);
        UpdateOrderCountText();
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
        UpdateOrderCountText();
    }

    public void UpdateOrderCountText()
    {
        int currentCount = _activeOrders.Count + _pendingOrders.Count;
        _ordersCountText.text = "Orders:" + currentCount.ToString();
    }
}
