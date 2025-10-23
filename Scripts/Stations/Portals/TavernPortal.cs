using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TavernPortal : MonoBehaviour, IFreeInteractable
{
    private OrdersManager _ordersManager;
    private OrdersPanelUI _ordersPanelUI;
    private GameSceneManager _gameSceneManager;

    [Inject]
    public void Construct(OrdersManager ordersManager, OrdersPanelUI ordersPanelUI,
        GameSceneManager gameSceneManager)
    {
        _ordersManager = ordersManager;
        _ordersPanelUI = ordersPanelUI;
        _gameSceneManager = gameSceneManager;
    }

    private void Start()
    {
        StartCoroutine(GiveCompletedDishesRoutine(_ordersManager.CompletedDishes));
    }

    public void Interact()
    {
        _ordersPanelUI.Open();
    }

    public void EndDay()
    {
        _gameSceneManager.LoadHouseScene();
    }

    private IEnumerator GiveCompletedDishesRoutine(IReadOnlyList<DishData> dishesData)
    {
        foreach (var dishData in dishesData)
        {
            GameObject dish = Instantiate(dishData.DishPrefab);
            Vector3 spawnOffset = new Vector3(Random.Range(-1.5f, 1.5f), -2, 0);
            dish
                .GetComponent<BaseHoldItem>()
                .ArcAnimation.Animate(transform.position, spawnOffset);

            yield return new WaitForSeconds(1f);
        }
    }
}
