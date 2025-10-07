using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class SellerStopZone : MonoBehaviour
{
    [SerializeField] private float _minWaitTime;
    [SerializeField] private float _maxWaitTime;

    private SellerMovement _movement;
    private Shop _shop;
    private bool _canStop;

    [Inject]
    public void Contruct(Shop shop)
    {
        _shop = shop;
        _canStop = true;
    }

    private void Awake()
    {
        _movement = GetComponentInParent<SellerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerController _) && _canStop)
            StartCoroutine(PlayerNearby());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!_shop.IsOpen && collision.TryGetComponent(out PlayerController _) && _canStop)
            _movement.StartMovingAround();
    }

    private IEnumerator PlayerNearby()
    {
        _movement.StopMoving();
        yield return new WaitForSeconds(
            UnityEngine.Random.Range(_minWaitTime, _maxWaitTime));

        if (!_shop.IsOpen)
            _movement.StartMovingAround();
    }

    public void EnableStopMoving() => _canStop = true;

    public void DisableStopMoving() => _canStop = false;
}
