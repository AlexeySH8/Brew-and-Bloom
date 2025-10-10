using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class OrdersManager : MonoBehaviour
{
    [SerializeField] private List<DishData> _completedDishes = new List<DishData>();

    public IReadOnlyList<DishData> CompletedDishes => _completedDishes;
    public IReadOnlyList<Order> ActiveOrders => _activeOrders;

    public event Action<Order> OnOrderAccepted;
    public event Action<Order> OnOrderCompleted;
    public event Action OnOrdersCleared;

    private List<Order> _activeOrders = new List<Order>();
    private GuestsManager _guestsManager;
    private Coroutine _acceptOrdersRoutine;

    [Inject]
    public void Construct(GuestsManager guestsManager)
    {
        _guestsManager = guestsManager;
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        _guestsManager.OnGuestsArrived += AcceptOrders;
        OnOrdersCleared += ClearCompletedDishes;
    }

    private void OnDisable()
    {
        _guestsManager.OnGuestsArrived -= AcceptOrders;
        OnOrdersCleared -= ClearCompletedDishes;
    }

    private void AcceptOrders(IReadOnlyList<Guest> guests)
    {
        OnOrdersCleared?.Invoke();

        if (_acceptOrdersRoutine != null)
            StopCoroutine(_acceptOrdersRoutine);

        _acceptOrdersRoutine = StartCoroutine(AcceptOrdersRoutine(guests));
    }

    private IEnumerator AcceptOrdersRoutine(IReadOnlyList<Guest> guests)
    {
        foreach (var guest in guests)
        {
            _activeOrders.Add(guest.CurrentOrder);
            OnOrderAccepted?.Invoke(guest.CurrentOrder);
            yield return new WaitForSeconds(1f);
        }
    }

    public void AddPlayerDish(DishData dishData)
    {
        _completedDishes.Add(dishData);
        if (IsDishInOrders(dishData, out Order order))
        {
            order.IsCompleted = true;
            OnOrderCompleted?.Invoke(order);
            _activeOrders.Remove(order);
        }
    }

    private bool IsDishInOrders(DishData dishData, out Order order)
    {
        order = _activeOrders.FirstOrDefault(
            activeOrder => activeOrder.Dish.IngredientsMask == dishData.IngredientsMask);
        return order != null;
    }

    private void ClearCompletedDishes() { _completedDishes.Clear(); }
}
