using System.Collections;
using UnityEngine;

public class But : MonoBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] float _minTimeNextPoint;
    [SerializeField] float _maxTimeNextPoint;

    private ButPen _butPen;
    private Vector2 _targetPos;
    private SpriteRenderer _spriteRenderer;

    public void Init(ButPen butPen)
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _butPen = butPen;
        StartMovingAround();
    }

    public void StartMovingAround() => StartCoroutine(MovingAround());

    private IEnumerator MovingAround()
    {
        _targetPos = _butPen.GetRandomPositionInArea();
        while (true)
        {
            yield return Move();
            yield return new WaitForSeconds(
                Random.Range(_minTimeNextPoint, _maxTimeNextPoint));
            _targetPos = _butPen.GetRandomPositionInArea();
        }
    }

    private IEnumerator Move()
    {
        float direction = Mathf.Sign(_targetPos.x - transform.position.x);
        FlipVisual(direction);

        Vector2 velocity = Vector2.zero;
        while (Vector2.Distance(transform.position, _targetPos) > 0.01f)
        {
            transform.position = Vector2.SmoothDamp(
            transform.position, _targetPos,
            ref velocity, 0.3f, _speed);
            yield return null;
        }
    }

    public void FlipVisual(float direction)
    {
        _spriteRenderer.flipX = direction < 0;
    }
}
