using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowderSpawner : MonoBehaviour
{
    [SerializeField] private int _maxPowderCount;
    [SerializeField] private GameObject _powderPrefab;
    [SerializeField] private float _minTimeToSpawn;
    [SerializeField] private float _maxTimeToSpawn;

    public List<Coroutine> Spawners { get; private set; }
    private BoxCollider2D _area;
    private int _currentPowderCount;


    private void Awake()
    {
        _area = GetComponent<BoxCollider2D>();
        Spawners = new List<Coroutine>();
        _currentPowderCount = 0;
    }

    public void AddSpawner()
    {
        Spawners.Add(StartCoroutine(SpawnPowder()));
    }

    private IEnumerator SpawnPowder()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(
                _minTimeToSpawn, _maxTimeToSpawn));

            if (_currentPowderCount < _maxPowderCount)
            {
                Powder powder = Instantiate(_powderPrefab, GetRandomPositionInArea(),
                Quaternion.identity).GetComponent<Powder>();
                powder.Init(this);

                _currentPowderCount++;
            }
        }
    }

    public Vector2 GetRandomPositionInArea()
    {
        Vector2 center = _area.bounds.center;
        Vector2 size = _area.bounds.size;
        float x = Random.Range(center.x - size.x / 2, center.x + size.x / 2);
        float y = Random.Range(center.y - size.y / 2, center.y + size.y / 2);
        return new Vector2(x, y);
    }

    public void OnPowderDestroy()
    {
        _currentPowderCount = Mathf.Max(0, _currentPowderCount - 1);
    }
}
