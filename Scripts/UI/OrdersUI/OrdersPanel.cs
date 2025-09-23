using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrdersPanel : MonoBehaviour
{
    [SerializeField] private Transform _content;
    [SerializeField] private GameObject _orderTemplatePref;

    private Dictionary<Guest, GameObject> _activeOrders;

    public static OrdersPanel Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void AddOrders(List<Guest> guests)
    {
        Clear();
        foreach (Guest guest in guests)
        {
            Order order = guest.CurrentOrder;
            GameObject orderTemplate = Instantiate(_orderTemplatePref, _content);
            _activeOrders.Add(order.Guest, orderTemplate);
            orderTemplate
                .GetComponent<OrderTemplate>()
                .DisplayOrder(order);
        }
    }

    private void Clear()
    {
        if (_activeOrders != null && _activeOrders.Count > 0)
            foreach (GameObject order in _activeOrders.Values)
                Destroy(order);
        _activeOrders = new Dictionary<Guest, GameObject>();
    }

    public void UpdateStatusOrder(bool isOrderCompleted, Guest guest)
    {
        OrderTemplate orderTemplate =
            _activeOrders[guest]
            ?.GetComponent<OrderTemplate>();

        if (orderTemplate == null)
        {
            Debug.LogError($"{guest.ToString()} orderTemplate not found");
            return;
        }

        orderTemplate.SetStatus(isOrderCompleted);
    }
}
