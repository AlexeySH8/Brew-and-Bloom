using System.Collections;
using UnityEngine;
using Zenject;

public class GrowPlant : MonoBehaviour
{
    public bool IsWaterNeed { get; private set; }

    [SerializeField] private int _minHarvestCount = 1;
    [SerializeField] private int _maxHarvestCount = 3;
    [SerializeField] private float _waterNeedChance = 0.1f;
    [SerializeField] private int _minTimeToDryOut = 5;
    [SerializeField] private int _maxTimeToDryOut = 7;

    private Soil _soil;
    private SeedData _seedData;
    private SoilVisual _soilVisual;
    private Coroutine _growingPlant;
    private Coroutine _dryOutRoutine;
    private GameSceneManager _gameSceneManager;

    [Inject]
    public void Construct(GameSceneManager gameSceneManager)
    {
        _gameSceneManager = gameSceneManager;
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        _gameSceneManager.OnHouseUnloading += EndGrowPlant;
    }

    private void Awake()
    {
        _soil = GetComponent<Soil>();
        _soilVisual = GetComponent<SoilVisual>();
    }

    public void PlantSeed(SeedData seedData)
    {
        if (seedData == null || _growingPlant != null) return;
        _seedData = seedData;
        _growingPlant = StartCoroutine(GrowPlantRoutine());
    }

    private IEnumerator GrowPlantRoutine()
    {
        foreach (Sprite stage in _seedData.GrowthStageSprites)
        {
            _soilVisual.UpdateGrowPlantStage(stage);
            MaybeRequireWater();

            yield return new WaitUntil(() => !IsWaterNeed);
            yield return new WaitForSeconds(Random.Range(_seedData.MinStageTime, _seedData.MaxStageTime));
        }
        yield return new WaitForSeconds(Random.Range(_seedData.MinStageTime, _seedData.MaxStageTime));
        EndGrowPlant();
    }

    private void MaybeRequireWater()
    {
        bool isWaterNeed = Random.value < _waterNeedChance;

        if (isWaterNeed)
        {
            SetPlantNeedWater(isWaterNeed);
            _dryOutRoutine = StartCoroutine(PlantDriedOut());
        }
    }

    private IEnumerator PlantDriedOut()
    {
        yield return new WaitForSeconds(
            Random.Range(_minTimeToDryOut, _maxTimeToDryOut));
        _seedData = null;
        EndGrowPlant();
    }

    public void SetPlantNeedWater(bool isWaterNeed)
    {
        if (IsWaterNeed == isWaterNeed) return;

        if (!isWaterNeed && _dryOutRoutine != null)
        {
            StopCoroutine(_dryOutRoutine);
            _dryOutRoutine = null;
        }

        IsWaterNeed = isWaterNeed;
        _soilVisual.SetWaterNeedIcon(IsWaterNeed);
        _soil.UpdateLayer();
    }

    private void EndGrowPlant()
    {
        if (_seedData == null) return;
        SpawnHarvest();

        if (_growingPlant != null)
        {
            StopCoroutine(_growingPlant);
            _growingPlant = null;
        }

        IsWaterNeed = false;

        _soil.UpdateLayer();
        _soil.StartStageReset();
        _soilVisual.ClearContentPlace();
    }

    private void SpawnHarvest()
    {
        for (int i = 0; i < Random.Range(_minHarvestCount, _maxHarvestCount + 1); i++)
            Instantiate(_seedData.IngredientPrefab, transform.position, transform.rotation);
        _seedData = null;
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }

    private void UnsubscribeFromEvents()
    {
        _gameSceneManager.OnHouseUnloading -= EndGrowPlant;
    }
}
