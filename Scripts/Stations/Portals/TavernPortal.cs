using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

            if (!dish.TryGetComponent(out ArcSpawnAnimation arc))
                arc = dish.AddComponent<ArcSpawnAnimation>();

            arc.LaunchFrom(transform.position);
            yield return new WaitForSeconds(1f);
        }
    }
}
