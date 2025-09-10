using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OrderVisual : MonoBehaviour
{
    [SerializeField] private OrderTemplate[] _orderTemplates;

    private Dictionary<Guest, OrderTemplate> _orderTemplatesDic = new Dictionary<Guest, OrderTemplate>();
    private List<Order> _pendingOrders = new List<Order>();

    public void AddOrder(Order order)
    {
        foreach (OrderTemplate template in _orderTemplates)
        {
            if (!template.HasOrder)
            {
                _orderTemplatesDic.Add(order.Guest, template);
                template.DisplayOrder(order.Dish);
                return;
            }
        }
        _pendingOrders.Add(order);
    }

    // ебаная проблема в том , что дик не переписывает гостя , когда заказ меняется 
    // я заебался сам поправишь удачи <3
    public void RemoveOrder(Order order)
    {
        Order pendingOrder = _pendingOrders.FirstOrDefault();
        if (pendingOrder == null)
        {
            _orderTemplatesDic[order.Guest].ClearVisual();
            _orderTemplatesDic.Remove(order.Guest);
        }
        else
        {
            _orderTemplatesDic.Remove(order.Guest);
            _pendingOrders.Remove(pendingOrder);
            _orderTemplatesDic[order.Guest].DisplayOrder(pendingOrder.Dish);
        }

    }
}
