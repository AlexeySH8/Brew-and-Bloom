using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class OrdersManager : MonoBehaviour, IDataPersistence
{
    [SerializeField] private List<DishData> _completedDishes = new List<DishData>();

    public IReadOnlyList<DishData> CompletedDishes => _completedDishes;
    public IReadOnlyList<Order> ActiveOrders => _activeOrders;

    public event Action<Order> OnOrderAccepted;
    public event Action<Order> OnOrderCompleted;
    public event Action OnOrdersCleared;

    private List<Order> _activeOrders = new List<Order>();
    private GuestsManager _guestsManager;
    private IDataPersistenceManager _dataPersistenceManager;
    private GameSceneManager _gameSceneManager;
    private Recipes _recipes;
    private Coroutine _acceptOrdersRoutine;

    [Inject]
    public void Construct(GuestsManager guestsManager, Recipes recipes,
        IDataPersistenceManager dataPersistenceManager, GameSceneManager gameSceneManager)
    {
        _dataPersistenceManager = dataPersistenceManager;
        _gameSceneManager = gameSceneManager;
        _guestsManager = guestsManager;
        _recipes = recipes;
        _dataPersistenceManager.Register(this);
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        _guestsManager.OnGuestsArrived += AcceptOrders;
        _gameSceneManager.OnTavernUnloading += _completedDishes.Clear;
    }

    private void OnDisable()
    {
        _guestsManager.OnGuestsArrived -= AcceptOrders;
        _gameSceneManager.OnTavernUnloading -= _completedDishes.Clear;
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
            if (guest.CurrentOrder.IsCompleted) continue;

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

    public void RemoveCompletedDish(DishData dishData)
    {
        if (_completedDishes.Contains(dishData))
            _completedDishes.Remove(dishData);
    }

    private bool IsDishInOrders(DishData dishData, out Order order)
    {
        order = _activeOrders.FirstOrDefault(
            activeOrder => activeOrder.Dish.IngredientsMask == dishData.IngredientsMask);
        return order != null;
    }

    public void LoadData(GameData gameData)
    {
        if (_completedDishes.Count > 0) return;
        foreach (var dishSave in gameData.CompletedDishes)
        {
            if (_recipes.TryGetDish(dishSave, out GameObject dishObj))
            {
                DishData dishData = dishObj.GetComponent<Dish>().Data;
                _completedDishes.Add(dishData);
            }
            else
                Debug.LogError("Non-existent IngredientMask was saved");
        }
    }

    public void SaveData(GameData gameData)
    {
        List<int> completedDishesToSave = new List<int>();
        foreach (var dish in _completedDishes)
            completedDishesToSave.Add(dish.IngredientsMask);
        gameData.CompletedDishes = completedDishesToSave;
    }

    private void OnDestroy()
    {
        _dataPersistenceManager.Unregister(this);
    }
}
