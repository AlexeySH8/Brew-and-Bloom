using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Mesh;

public class TavernPortal : MonoBehaviour, IFreeInteractable
{
    private OrdersPanelUI _ordersPanel;
    [SerializeField] GameObject _testPrefab;

    private void Awake()
    {
        _ordersPanel = FindAnyObjectByType<OrdersPanelUI>();
    }

    private void Start()
    {
        StartCoroutine(GiveCompletedDishesRoutine());
    }

    public void Interact()
    {
        _ordersPanel.Open();
    }

    public void EndDay()
    {
        DayManager.Instance.EndDay();
        SceneManager.LoadScene("House");
    }

    private IEnumerator GiveCompletedDishesRoutine()
    {
        for (int i = 0; i < 5; i++)
        {
            //GameObject dish = Instantiate(_testPrefab);

            //if (!dish.TryGetComponent(out ArcSpawnAnimation arc))
            //    arc = dish.AddComponent<ArcSpawnAnimation>();

            //arc.LaunchFrom(transform.position);
            yield return new WaitForSeconds(1f);
        }
    }
}
