using System.Collections;
using UnityEngine;
using Zenject;

public class SellerMovement : MonoBehaviour
{
    [Header("Moving Around")]
    [SerializeField] private float _normalSpeed;
    [SerializeField] private Vector2 _minBounds;
    [SerializeField] private Vector2 _maxBounds;
    [SerializeField] private float _minTimeNextPoint;
    [SerializeField] private float _maxTimeNextPoint;
    [Header("Delivery")]
    [SerializeField] private float _deliverySpeed;
    [SerializeField] private float _radius;
    [SerializeField] private Vector2 _deliveryPoint;

    private Shop _shop;
    private float _currentSpeed;
    private Coroutine _movingRoutine;
    private Vector2 _targetPos;
    private SellerStopZone _stopZone;
    private SellerVisual _maneShopVisual;

    [Inject]
    public void Construct(Shop shop)
    {
        _shop = shop;
    }

    private void Awake()
    {
        transform.position = GetRandomPosition();
        _stopZone = GetComponentInChildren<SellerStopZone>();
        _maneShopVisual = GetComponentInChildren<SellerVisual>();
    }

    private void Start()
    {
        _targetPos = GetRandomPosition();
        StartMovingAround();
    }

    public void StartMovingAround()
    {
        if (_movingRoutine != null) return;

        _stopZone.EnableStopMoving();
        _movingRoutine = StartCoroutine(MovingAround());
    }

    public void StopMoving()
    {
        if (_movingRoutine == null) return;

        StopCoroutine(_movingRoutine);
        _movingRoutine = null;
    }

    public IEnumerator StartDeliver()
    {
        _stopZone.DisableStopMoving();
        StopMoving();

        _currentSpeed = _deliverySpeed;
        _targetPos = GetRandomPickupPoint();
        yield return Move();

        _targetPos = _deliveryPoint;
        yield return Move();
    }

    private IEnumerator MovingAround()
    {
        _currentSpeed = _normalSpeed;
        _targetPos = GetRandomPosition();
        while (true)
        {
            yield return Move();
            yield return new WaitForSeconds(
                Random.Range(_minTimeNextPoint, _maxTimeNextPoint));
            _targetPos = GetRandomPosition();
        }
    }

    private IEnumerator Move()
    {
        float direction = Mathf.Sign(_targetPos.x - transform.position.x);
        _maneShopVisual.FlipVisual(direction);

        Vector2 velocity = Vector2.zero;
        while (Vector2.Distance(transform.position, _targetPos) > 0.01f)
        {
            transform.position = Vector2.SmoothDamp(
            transform.position, _targetPos,
            ref velocity, 0.3f, _currentSpeed);
            yield return null;
        }
    }

    private Vector2 GetRandomPosition()
    {
        return new Vector2(
            Random.Range(_minBounds.x, _maxBounds.x),
            Random.Range(_minBounds.y, _maxBounds.y));
    }

    private Vector2 GetRandomPickupPoint()
    {
        float angle = Random.Range(0f, Mathf.PI * 2f);
        Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        return (Vector2)transform.position + direction * _radius;
    }
}
