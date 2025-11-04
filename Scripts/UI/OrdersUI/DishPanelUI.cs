using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class DishPanelUI : MonoBehaviour
{
    [SerializeField] private Transform _content;
    [SerializeField] private GameObject _dishTemplatePref;
    [SerializeField] private TextMeshProUGUI _ordersCountText;

    private Dictionary<Guest, GameObject> _activeDishes = new Dictionary<Guest, GameObject>();
    private OrdersManager _ordersManager;

    [Inject]
    public void Construct(OrdersManager ordersManager)
    {
        _ordersManager = ordersManager;
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        _ordersManager.OnOrderAccepted += AddOrder;
        _ordersManager.OnOrderCompleted += RemoveOrder;
        _ordersManager.OnOrdersCleared += Clear;
    }

    private void OnDisable()
    {
        _ordersManager.OnOrderAccepted -= AddOrder;
        _ordersManager.OnOrderCompleted -= RemoveOrder;
        _ordersManager.OnOrdersCleared -= Clear;
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
