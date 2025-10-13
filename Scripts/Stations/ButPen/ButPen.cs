using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButPen : MonoBehaviour
{
    [SerializeField] private int _maxButCount;
    [SerializeField] private GameObject _butPrefab;

    private PowderSpawner _powderSpawners;
    private BoxCollider2D _area;

    private void Awake()
    {
        _area = GetComponent<BoxCollider2D>();
        _powderSpawners = GetComponentInChildren<PowderSpawner>();
    }

    [ContextMenu("Spawn But")]
    public bool TrySpawnBut()
    {
        if (_powderSpawners.Spawners.Count >= _maxButCount) return false;

        _powderSpawners.AddSpawner();

        But but = Instantiate(_butPrefab, transform)
            .GetComponent<But>();
        but.Init(this);

        return true;
    }

    public Vector2 GetRandomPositionInArea()
    {
        Vector2 center = _area.bounds.center;
        Vector2 size = _area.bounds.size;
        float x = Random.Range(center.x - size.x / 2, center.x + size.x / 2);
        float y = Random.Range(center.y - size.y / 2, center.y + size.y / 2);
        return new Vector2(x, y);
    }
}
