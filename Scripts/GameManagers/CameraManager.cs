using System;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _xBoundary;
    [SerializeField] private float _yBoundary;
    [SerializeField] private float _smoothTime;

    private Vector3 _initialPos;
    private Vector3 _velocity;
    private bool isObserved = true;

    private void Awake()
    {
        _initialPos = transform.position;
    }

    private void LateUpdate()
    {
        if (isObserved)
            ObserveTarget();
    }

    private void ObserveTarget()
    {
        Vector3 targetPos = _initialPos;

        if (Math.Abs(_target.position.x) > _xBoundary)
            targetPos.x = _target.position.x - Math.Sign(_target.position.x) * _xBoundary;
        if (Math.Abs(_target.position.y) > _yBoundary)
            targetPos.y = _target.position.y - Math.Sign(_target.position.y) * _yBoundary;

        transform.position = Vector3.SmoothDamp(transform.position, targetPos,
                ref _velocity, _smoothTime); 
    }
}
