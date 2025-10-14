using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class HousePortal : MonoBehaviour, IReceiveHeldItem, IFreeInteractable
{
    private OrdersManager _ordersManager;
    private GameSceneManager _gameSceneManager;

    [Inject]
    public void Construct(OrdersManager ordersManager, GameSceneManager gameSceneManager)
    {
        _ordersManager = ordersManager;
        _gameSceneManager = gameSceneManager;
    }

    public void Interact()
    {
        LoadTavernScene();
    }

    public bool TryReceive(BaseHoldItem heldItem)
    {
        if (heldItem.TryGetComponent(out Dish dish))
        {
            _ordersManager.AddPlayerDish(dish.Data);
            dish.Discard();
            return true;
        }
        else
        {
            LoadTavernScene();
        }
        return false;
    }

    private void LoadTavernScene()
    {
        _gameSceneManager.LoadTavernScene();
    }
}
