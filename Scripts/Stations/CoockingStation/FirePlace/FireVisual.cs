using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireVisual : MonoBehaviour
{
    [SerializeField] private Image _fillTemperature;
    private const string IsFireEnd = "IsFireEnd";
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void UpdateTemperatureVisual(float intensity)
    {
        _fillTemperature.fillAmount = Mathf.Clamp01(intensity / 100f);
    }

    public void StartFire()
    {
        _animator.SetBool(IsFireEnd, false);
    }

    public void EndFire()
    {
        _animator.SetBool(IsFireEnd, true);
    }
}
