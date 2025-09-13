using System;
using System.Collections;
using UnityEngine;

public class ManeStopZone : MonoBehaviour
{
    [SerializeField] private Shop _shop;
    [SerializeField] private float _minWaitTime;
    [SerializeField] private float _maxWaitTime;
    private ManeMovement _movement;
    private bool _canStop = true;

    private void Awake()
    {
        _movement = GetComponentInParent<ManeMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerController _) && _canStop)
            StartCoroutine(PlayerNearby());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerController _) && _canStop)
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
