using System;
using UnityEngine;

public class StopZone : MonoBehaviour
{
    private ManeShopMovement _movement;
    private bool _canStop = true;

    private void Awake()
    {
        _movement = GetComponentInParent<ManeShopMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerController _) && _canStop)
            _movement.StopMoving();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerController _) && _canStop)
            _movement.StartMovingAround();
    }

    public void EnableStopMoving() => _canStop = true;

    public void DisableStopMoving() => _canStop = false;
}
