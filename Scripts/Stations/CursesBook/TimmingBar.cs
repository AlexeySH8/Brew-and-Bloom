using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimmingBar : MonoBehaviour
{
    [SerializeField] private float _speedDefault;
    [SerializeField] private float _stepDelayDefault;
    [SerializeField] private RectTransform _bar;
    [SerializeField] private RectTransform _successZone;
    [SerializeField] private RectTransform _indicator;

    private float _currentSpeed, _currentStepDelay;
    private float _indicatorXBoundary, _zoneXBoundary;
    private bool _movingRight = true;
    Coroutine _moving;

    private void Awake()
    {
        _currentSpeed = _speedDefault;
        _currentStepDelay = _stepDelayDefault;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            CheckSuccess();
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
        gameObject.SetActive(false);
        StopCoroutine(_moving);
        _moving = null;
        _currentSpeed *= 2;
        _currentStepDelay /= 2;
    }

    private IEnumerator MoveIndicator()
    {
        while (true)
        {
            float step = _speedDefault * _stepDelayDefault * (_movingRight ? 1 : -1);
            _indicator.anchoredPosition += new Vector2(step, 0);

            if (_indicator.anchoredPosition.x >= _indicatorXBoundary)
                _movingRight = false;
            else if (_indicator.anchoredPosition.x <= -_indicatorXBoundary)
                _movingRight = true;

            yield return new WaitForSeconds(_stepDelayDefault);
        }
    }

    public bool CheckSuccess()
    {
        float zoneLeft = _successZone.anchoredPosition.x - _successZone.rect.width / 2;
        float zoneRight = _successZone.anchoredPosition.x + _successZone.rect.width / 2;

        float indicatorLeft = _indicator.anchoredPosition.x - _indicator.rect.width / 2;
        float indicatorRight = _indicator.anchoredPosition.x + _indicator.rect.width / 2;

        bool isSuccess = zoneLeft <= indicatorLeft && zoneRight >= indicatorRight;
        if (isSuccess)
        {
            Debug.Log("WIN");
        }
        else
            Debug.Log("LOSE");
        SetSuccessZone();
        return isSuccess;
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
