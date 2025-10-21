using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcAnimation : MonoBehaviour
{
    private float _height;       // ������ ����
    private float _duration;   // ����� ������
    private Vector3 _offset; // ���� ��������� ������ �� �������

    private Vector3 _startPos;
    private Vector3 _endPos;

    public void Animate(Vector3 startPosition, Vector3 offset,
        float height = 1f, float duration = 0.5f)
    {
        _height = height;
        _duration = duration;
        _offset = offset;
        _startPos = startPosition;
        _endPos = startPosition + _offset;
        transform.position = _startPos;
        StartCoroutine(MoveArc());
    }

    private IEnumerator MoveArc()
    {
        float elapsed = 0f;

        while (elapsed < _duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / _duration);

            // �������� ������������ �� XZ
            Vector3 pos = Vector3.Lerp(_startPos, _endPos, t);

            // ��������� �������� �� Y
            pos.y += Mathf.Sin(t * Mathf.PI) * _height;

            transform.position = pos;
            yield return null;
        }

        transform.position = _endPos;
    }
}
