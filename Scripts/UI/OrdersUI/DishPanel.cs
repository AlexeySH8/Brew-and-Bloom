using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DishPanel : MonoBehaviour
{
    [SerializeField] private Transform _content;
    [SerializeField] private GameObject _dishTemplatePref;
    [SerializeField] private TextMeshProUGUI _ordersCountText;

    private Dictionary<Guest, GameObject> _activeDishes = new Dictionary<Guest, GameObject>();

    public void AddOrder(Order order)
    {
        GameObject orderTemplate = Instantiate(_dishTemplatePref, _content);
        _activeDishes.Add(order.Guest, orderTemplate);
        orderTemplate
            .GetComponent<DishTemplate>()
            .DisplayDish(order.Dish);
        UpdateOrderCountText();
    }

    public void RemoveOrder(Order order)
    {
        GameObject orderTemplate = _activeDishes[order.Guest];
        Destroy(orderTemplate);
        _activeDishes.Remove(order.Guest);
        UpdateOrderCountText();
    }

    public void UpdateOrderCountText()
    {
        int currentCount = _activeDishes.Count;
        _ordersCountText.text = "Orders:" + currentCount.ToString();
    }
}
