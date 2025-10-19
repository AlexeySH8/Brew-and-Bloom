using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour, IStaffTarget
{
    [SerializeField, Range(0f, 99f)]
    private float _minIntensity;
    [SerializeField] private float _igniteRate;
    [SerializeField] private float _decayRate;

    private CookingStation _firePlace;
    private FireVisual _fireVisual;
    private float _currentIntensity;
    private float _maxIntensity = 100;
    private bool _isFireEnd = false;

    private void Awake()
    {
        _firePlace = GetComponentInParent<CookingStation>();
        _fireVisual = GetComponent<FireVisual>();
        _currentIntensity = _maxIntensity;
        _fireVisual.StartFire();
    }

    private void Update()
    {
        Decay();

        if (_currentIntensity <= _minIntensity)
            EndFire();
        else if (_isFireEnd && _currentIntensity > _minIntensity)
            StartFire();

        _fireVisual.UpdateTemperatureVisual(_currentIntensity);
    }

    private void Decay()
    {
        float intensity = _currentIntensity - _decayRate * Time.deltaTime;
        _currentIntensity = Mathf.Max(0, intensity);
    }

    private void Ignite()
    {
        float intensity = _currentIntensity + _igniteRate;
        _currentIntensity = Mathf.Min(_maxIntensity, intensity);
    }

    private void StartFire()
    {
        _isFireEnd = false;
        _firePlace.IsCookingStop = false;
        _fireVisual.StartFire();
    }

    private void EndFire()
    {
        _isFireEnd = true;
        _firePlace.IsCookingStop = true;
        _fireVisual.EndFire();
    }

    public void InteractWithStaff() => Ignite();
}
