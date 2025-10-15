using UnityEngine;

public class CauldronDishHolder : MonoBehaviour
{
    [SerializeField] private Vector2 _minPos;
    [SerializeField] private Vector2 _maxPos;
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _rotationSpeed = 1f;
    [SerializeField] private float _rotationAngle = 20f;

    private void Update()
    {
        float t = Mathf.PingPong(Time.time * _speed, 1f);
        Vector2 newPos = Vector2.Lerp(_minPos, _maxPos, t);
        transform.localPosition = newPos;

        float rot = Mathf.Sin(Time.time * _rotationSpeed) * _rotationAngle;
        transform.localRotation = Quaternion.Euler(0f, 0f, rot);
    }
}
