using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcSpawnAnimation : MonoBehaviour
{
    [SerializeField] private float _height = 1f;       // ������ ����
    [SerializeField] private float _duration = 0.5f;   // ����� ������
    [SerializeField] private Vector3 _offset; // ���� ��������� ������ �� �������

    private Vector3 _startPos;
    private Vector3 _endPos;

    public void LaunchFrom(Vector3 startPosition)
    {
        _offset = new Vector3(Random.Range(-2f, 2f), -1.5f, 0);
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
