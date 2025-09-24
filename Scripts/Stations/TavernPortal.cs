using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Mesh;

public class TavernPortal : MonoBehaviour, IFreeInteractable
{
    private OrdersPanel _ordersPanel;
    [SerializeField] GameObject _testPrefab;

    private void Awake()
    {
        _ordersPanel = FindAnyObjectByType<OrdersPanel>();
    }

    private void Start()
    {
        StartCoroutine(GiveCompletedDishesRoutine());
    }

    public void Interact()
    {
        _ordersPanel.Open();
    }

    private IEnumerator GiveCompletedDishesRoutine()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject dish = Instantiate(_testPrefab);

            if (!dish.TryGetComponent(out ArcSpawnAnimation arc))
                arc = dish.AddComponent<ArcSpawnAnimation>();

            arc.LaunchFrom(transform.position);
            yield return new WaitForSeconds(1f);
        }
    }
}
