using UnityEngine;

public class CauldronDishHolder : MonoBehaviour
{
    [SerializeField] private float _xBoundary;
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _rotationSpeed = 1f;
    [SerializeField] private float _rotationAngle = 20f;
    [SerializeField] private float _yAmplitude = 0.2f;
    [SerializeField] private float _yFrequency = 2f;

    private float _baseY;

    private void Start()
    {
        _baseY = transform.localPosition.y;
    }

    private void Update()
    {
        float t = Mathf.PingPong(Time.time * _speed, 1f);
        float newX = Mathf.Lerp(-_xBoundary, _xBoundary, t);

        float newY = _baseY + Mathf.Sin(Time.time * _yFrequency) * _yAmplitude;

        transform.localPosition = new Vector2(newX, newY);

        float rot = Mathf.Sin(Time.time * _rotationSpeed) * _rotationAngle;
        transform.localRotation = Quaternion.Euler(0f, 0f, rot);
    }
}
