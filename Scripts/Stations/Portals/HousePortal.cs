using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class HousePortal : MonoBehaviour, IReceiveHeldItem, IFreeInteractable
{
    private OrdersManager _ordersManager;

    [Inject]
    public void Construct(OrdersManager ordersManager)
    {
        _ordersManager = ordersManager;
    }

    public void Interact()
    {
        LoadTavernScene();
    }

    public void Receive(GameObject heldItem)
    {
        if (heldItem.TryGetComponent(out Dish dish))
        {
            _ordersManager.AddPlayerDish(dish.Data);
            dish.GetComponent<BaseHoldItem>().Discard();
        }
        else
        {
            LoadTavernScene();
        }
    }

    private void LoadTavernScene()
    {
        SceneManager.LoadScene("Tavern");
    }
}
