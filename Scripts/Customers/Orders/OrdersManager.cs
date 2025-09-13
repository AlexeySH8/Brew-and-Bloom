using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OrdersManager : MonoBehaviour
{
    [SerializeField] private List<DishData> _completedDishes = new List<DishData>();
    private List<Order> _activeOrders = new List<Order>();

    private OrderVisual _orderVisual;

    private void Awake()
    {
        _orderVisual = GetComponent<OrderVisual>();
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
        _activeOrders.Add(guest.CurrentOrder);
        _orderVisual.AddOrder(guest.CurrentOrder);
    }

    public void AddPlayerDish(DishData dishData)
    {
        _completedDishes.Add(dishData);
        if (IsDishInOrders(dishData, out Order order))
        {
            order.MarkAsCompleted();
            _orderVisual.RemoveOrder(order);
            _activeOrders.Remove(order);
        }
    }

    private bool IsDishInOrders(DishData dishData, out Order order)
    {
        order = _activeOrders.FirstOrDefault(
            activeOrder => activeOrder.Dish.IngredientsMask == dishData.IngredientsMask);
        return order != null;
    }
}
