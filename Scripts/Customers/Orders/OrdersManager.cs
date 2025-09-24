using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OrdersManager : MonoBehaviour
{
    public static OrdersManager Instance;

    [SerializeField] private List<DishData> _completedDishes = new List<DishData>();

    public event Action<Order> OnOrderAccepted;
    public event Action<Order> OnOrderCompleted;
    public event Action OnOrdersCleared;

    private List<Order> _activeOrders = new List<Order>();
    private Coroutine _acceptOrdersRoutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        GuestsManager.Instance.OnGuestsArrived += AcceptOrders;
    }

    private void OnDisable()
    {
        GuestsManager.Instance.OnGuestsArrived -= AcceptOrders;
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
            order.MarkAsCompleted();
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

    public IReadOnlyList<DishData> CompletedDishes() => _completedDishes;

    public IReadOnlyList<Order> ActiveOrders() => _activeOrders;
}
