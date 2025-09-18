using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoockingStationVisual : MonoBehaviour
{
    [Header("Station Contents")]
    [SerializeField] private float _yContainerPos = 1.05f;
    [SerializeField] private float _xDeltaContainerPos = 0.42f;
    [SerializeField] private GameObject _containerPrefab;
    [Header("Color Change")]
    [SerializeField] private SpriteRenderer _spriteColor;
    [SerializeField] private float _duration = 1f;

    private Color _original�olor;

    private List<GameObject> _containersList = new();

    private void Awake()
    {
        _original�olor = _spriteColor.color;
    }

    public void AddIngredient(Sprite ingredient)
    {
        GameObject container = Instantiate(_containerPrefab, transform);
        var a = new Vector2(GetXOffsetContainerPos(), _yContainerPos);
        container.transform.localPosition = new Vector2(GetXOffsetContainerPos(), _yContainerPos);
        container.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = ingredient;
        _containersList.Add(container);
    }

    public void ClearIngredients()
    {
        foreach (GameObject container in _containersList)
            Destroy(container);
        ChangeColorTo(_original�olor);
        _containersList.Clear();
    }

    public void ChangeColorTo(Color color)
    {
        if (color == Color.white) return;
        StartCoroutine(ChangeColorCoroutineTo(color));
    }

    private IEnumerator ChangeColorCoroutineTo(Color color)
    {
        Color startColor = _spriteColor.color;
        float time = 0;
        Color targetColor = new Color(color.r, color.b, color.g, _spriteColor.color.a);

        while (time < _duration)
        {
            time += Time.deltaTime;
            float t = time / _duration;
            _spriteColor.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }
        _spriteColor.color = targetColor;
    }

    private float GetXOffsetContainerPos()
    {
        int index = _containersList.Count;
        if (index > 0)
        {
            int direction = (index % 2 == 1) ? 1 : -1; // odd to the right, even to the left
            int step = (index + 1) / 2;
            return step * _xDeltaContainerPos * direction;
        }
        return 0;
    }
}
