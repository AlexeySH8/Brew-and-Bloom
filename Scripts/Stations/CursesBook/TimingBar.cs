using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimingBar : MonoBehaviour
{
    [SerializeField] private float _baseSpeed;
    [SerializeField] private float _baseStepDelay;
    [SerializeField] private float _difficultyMultiplier;
    [SerializeField] private RectTransform _bar;
    [SerializeField] private RectTransform _successZone;
    [SerializeField] private RectTransform _indicator;

    private float _currentSpeed, _currentStepDelay, _baseXZoneScale;
    private float _indicatorXBoundary, _zoneXBoundary;
    private bool _movingRight = true;
    Coroutine _moving;

    private void Awake()
    {
        _baseXZoneScale = _successZone.localScale.x;
        ResetDifficulty();
    }

    public void StartPlay()
    {
        gameObject.SetActive(true);
        _moving = StartCoroutine(MoveIndicator());
        CalculateBoundary();
        SetSuccessZone();
    }

    public void EndPlay()
    {
        StopCoroutine(_moving);
        _moving = null;
        IncreaseDifficulty();
        gameObject.SetActive(false);
    }

    private IEnumerator MoveIndicator()
    {
        while (true)
        {
            float step = _currentSpeed * _currentStepDelay * (_movingRight ? 1 : -1);
            _indicator.anchoredPosition += new Vector2(step, 0);

            if (_indicator.anchoredPosition.x >= _indicatorXBoundary)
                _movingRight = false;
            else if (_indicator.anchoredPosition.x <= -_indicatorXBoundary)
                _movingRight = true;

            yield return new WaitForSeconds(_currentStepDelay);
        }
    }

    public bool CheckSuccess()
    {
        float zoneLeft = _successZone.anchoredPosition.x - _successZone.rect.width / 2;
        float zoneRight = _successZone.anchoredPosition.x + _successZone.rect.width / 2;

        float indicatorLeft = _indicator.anchoredPosition.x - _indicator.rect.width / 2;
        float indicatorRight = _indicator.anchoredPosition.x + _indicator.rect.width / 2;

        bool isSuccess = zoneLeft <= indicatorLeft && zoneRight >= indicatorRight;
        SetSuccessZone();
        return isSuccess;
    }

    private void IncreaseDifficulty()
    {
        var zoneScale = _successZone.localScale;
        zoneScale.x = Mathf.Max(0.1f, _successZone.localScale.x / _difficultyMultiplier);
        _successZone.localScale = zoneScale;
        _currentSpeed *= _difficultyMultiplier;
        _currentStepDelay /= _difficultyMultiplier;
    }

    private void ResetDifficulty()
    {
        var zoneScale = _successZone.localScale;
        zoneScale.x = _baseXZoneScale;
        _successZone.localScale = zoneScale;
        _currentSpeed = _baseSpeed;
        _currentStepDelay = _baseStepDelay;
    }

    private void SetSuccessZone()
    {
        float randomX = Random.Range(-_zoneXBoundary, _zoneXBoundary);
        _successZone.anchoredPosition = new Vector2(randomX, 0);
    }

    private void CalculateBoundary()
    {
        float halfBar = _bar.rect.width / 2;
        float halfIndicator = _indicator.rect.width / 2;
        float halfZone = _successZone.rect.width / 2;

        _indicatorXBoundary = halfBar - halfIndicator;
        _zoneXBoundary = halfBar - halfZone;
    }
}
