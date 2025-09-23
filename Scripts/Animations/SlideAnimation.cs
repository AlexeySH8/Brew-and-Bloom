using System.Collections;
using UnityEngine;

public class SlideAnimation : MonoBehaviour
{
    [SerializeField] private Vector2 _startPosition;
    [SerializeField] private Vector2 _endPosition;
    [SerializeField] private float _time;
    [SerializeField] private float _stepTime;

    private Coroutine _transition;

    public bool IsTransitionOver() => _transition == null;

    public void Transition(bool isOpen) => _transition = StartCoroutine(TransitionRoutine(isOpen));

    private IEnumerator TransitionRoutine(bool isOpen)
    {
        if (_stepTime > _time)
        {
            Debug.LogError("Incorrect values ​​for SlideAnimation");
            yield break;
        }

        Vector2 direction = _endPosition - _startPosition;
        int stepsCount = Mathf.RoundToInt(_time / _stepTime);
        Vector2 step = direction / stepsCount;

        if (!isOpen) step *= -1f;

        Vector2 currentPos = isOpen ? _startPosition : _endPosition;
        transform.localPosition = currentPos;

        for (int i = 0; i < stepsCount; i++)
        {
            currentPos += step;
            transform.localPosition = currentPos;
            yield return new WaitForSeconds(_stepTime);
        }

        _transition = null;
    }
}
