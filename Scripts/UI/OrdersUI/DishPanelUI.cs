using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DishPanelUI : MonoBehaviour
{
    [SerializeField] private Transform _content;
    [SerializeField] private GameObject _dishTemplatePref;
    [SerializeField] private TextMeshProUGUI _ordersCountText;

    private Dictionary<Guest, GameObject> _activeDishes = new Dictionary<Guest, GameObject>();

    private void Start()
    {
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        if (OrdersManager.Instance == null) return;

        OrdersManager.Instance.OnOrderAccepted += AddOrder;
        OrdersManager.Instance.OnOrderCompleted += RemoveOrder;
        OrdersManager.Instance.OnOrdersCleared += Clear;
    }

    private void OnDisable()
    {
        OrdersManager.Instance.OnOrderAccepted -= AddOrder;
        OrdersManager.Instance.OnOrderCompleted -= RemoveOrder;
        OrdersManager.Instance.OnOrdersCleared -= Clear;
    }

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
        if (_activeDishes.TryGetValue(order.Guest, out GameObject orderTemplate))
        {
            Destroy(orderTemplate);
            _activeDishes.Remove(order.Guest);
            UpdateOrderCountText();
        }
    }

    public void Clear()
    {
        if (_activeDishes == null) return;

        if (_activeDishes.Count > 0)
            foreach (GameObject orderTemplate in _activeDishes.Values)
                Destroy(orderTemplate);

        _activeDishes = new Dictionary<Guest, GameObject>();
    }

    public void UpdateOrderCountText()
    {
        int currentCount = _activeDishes.Count;
        _ordersCountText.text = "Orders:" + currentCount.ToString();
    }
}
