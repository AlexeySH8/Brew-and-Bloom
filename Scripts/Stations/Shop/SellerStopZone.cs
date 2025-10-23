using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class SellerStopZone : MonoBehaviour
{
    [SerializeField] private float _minWaitTime;
    [SerializeField] private float _maxWaitTime;

    private Coroutine _waiting;
    private SellerMovement _movement;
    private Shop _shop;
    private bool _canStop;

    [Inject]
    public void Construct(Shop shop)
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
        if (collision.TryGetComponent(out PlayerController _) &&
            _canStop && _waiting == null)
            _waiting = StartCoroutine(PlayerNearby());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isActiveAndEnabled) return;
        if (collision.TryGetComponent(out PlayerController _) &&
            _canStop && !_shop.IsOpen)
            _movement.StartMovingAround();
    }

    private IEnumerator PlayerNearby()
    {
        _movement.StopMoving();
        yield return new WaitForSeconds(
            UnityEngine.Random.Range(_minWaitTime, _maxWaitTime));

        if (!_shop.IsOpen)
            _movement.StartMovingAround();

        _waiting = null;
    }

    public void EnableStopMoving() => _canStop = true;

    public void DisableStopMoving() => _canStop = false;
}
