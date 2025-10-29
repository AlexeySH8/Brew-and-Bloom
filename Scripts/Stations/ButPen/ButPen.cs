using UnityEngine;
using Zenject;

public class ButPen : MonoBehaviour, IDataPersistence
{
    [SerializeField] private int _maxButCount;
    [SerializeField] private GameObject _butPrefab;

    private PowderSpawner _powderSpawners;
    private IDataPersistenceManager _persistenceManager;
    private BoxCollider2D _area;

    [Inject]
    public void Construct(IDataPersistenceManager persistenceManager)
    {
        _persistenceManager = persistenceManager;
        _persistenceManager.Register(this);
    }

    private void Awake()
    {
        _area = GetComponent<BoxCollider2D>();
        _powderSpawners = GetComponentInChildren<PowderSpawner>();
    }

    private void Start()
    {
        if (_powderSpawners.Spawners.Count == 0)
            TrySpawnBut();
    }

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

    public void LoadData(GameData gameData)
    {
        int currentCount = _powderSpawners.Spawners.Count;
        int target = gameData.PowderSpawnersCount;
        int needToSpawn = target - currentCount;
        for (int i = 0; i < needToSpawn; i++)
            TrySpawnBut();
    }

    public void SaveData(GameData gameData)
    {
        gameData.PowderSpawnersCount = _powderSpawners.Spawners.Count;
    }

    private void OnDestroy()
    {
        _persistenceManager.Unregister(this);
    }
}
