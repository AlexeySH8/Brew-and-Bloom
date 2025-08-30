using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManeShopMovement : MonoBehaviour
{
    [Header("Moving Around")]
    [SerializeField] private float _normalSpeed;
    [SerializeField] private Vector2 _minBounds;
    [SerializeField] private Vector2 _maxBounds;
    [Header("Delivery")]
    [SerializeField] private float _deliverySpeed;
    [SerializeField] private float _radius;
    [SerializeField] private Vector2 _deliveryPoint;

    private float _currentSpeed;
    private Coroutine _movingRoutine;
    private Vector2 _targetPos;
    private StopZone _stopZone;

    private void Awake()
    {
        _stopZone = GetComponentInChildren<StopZone>();
    }

    private void Start()
    {
        _currentSpeed = _normalSpeed;
        _targetPos = GetRandomTargetPos();
        StartMovingAround();
    }

    public void StartMovingAround()
    {
        if (_movingRoutine == null)
            _movingRoutine = StartCoroutine(MovingAround());
    }

    public void StopMoving()
    {
        if (_movingRoutine != null)
        {
            StopCoroutine(_movingRoutine);
            _movingRoutine = null;
        }
    }

    public IEnumerator StartDeliver()
    {
        _stopZone.DisableStopMoving();
        StopMoving();

        _currentSpeed = _deliverySpeed;
        _targetPos = GetRandomPickupPoint();
        yield return Moving();

        _targetPos = _deliveryPoint;
        yield return Moving();       
    }

    public void EndDeliver()
    {
        StartMovingAround();
        _stopZone.EnableStopMoving();
    }

    private IEnumerator MovingAround()
    {
        _currentSpeed = _normalSpeed;
        _targetPos = GetRandomTargetPos();
        while (true)
        {
            yield return Moving();
            yield return new WaitForSeconds(1);
            _targetPos = GetRandomTargetPos();
        }
    }

    private IEnumerator Moving()
    {
        while (!Move())
        {
            yield return null;
        }
        yield break;
    }

    private bool Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, _targetPos, _currentSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, _targetPos) < 0.01f)
            return true;

        return false;
    }

    private Vector2 GetRandomTargetPos()
    {
        return new Vector2(
            Random.Range(_minBounds.x, _maxBounds.x),
            Random.Range(_minBounds.y, _maxBounds.y));
    }

    private Vector2 GetRandomPickupPoint()
    {
        float angle = Random.Range(0f, 360f) * Mathf.Rad2Deg;
        Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        return (Vector2)transform.position + direction * _radius;
    }
}
