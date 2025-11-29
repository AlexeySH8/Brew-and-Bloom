using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TavernPortal : MonoBehaviour, IFreeInteractable
{
    [SerializeField] private AudioClip _giveDishSFX;

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
        StartCoroutine(GiveCompletedDishesRoutine());
    }

    public void Interact()
    {
        _ordersPanelUI.Open();
    }

    public void EndDay()
    {
        SFX.Instance.PlayClickButtonNewDay();
        _gameSceneManager.LoadHouseScene();
    }

    private IEnumerator GiveCompletedDishesRoutine()
    {
        var completedDishes = new List<DishData>(_ordersManager.CompletedDishes);
        foreach (var dishData in completedDishes)
        {
            SFX.Instance.PlayAudioClip(_giveDishSFX);
            GameObject dish = Instantiate(dishData.DishPrefab);
            Vector3 spawnOffset = new Vector3(Random.Range(-1.5f, 1.5f), -2, 0);
            dish
                .GetComponent<BaseHoldItem>()
                .ArcAnimation.Animate(transform.position, spawnOffset);
            _ordersManager.RemoveCompletedDish(dishData);
            yield return new WaitForSeconds(1f);
        }
    }
}
