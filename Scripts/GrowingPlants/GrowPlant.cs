using System.Collections;
using UnityEditor.SceneManagement;
using UnityEngine;
using Zenject;

public class GrowPlant : MonoBehaviour
{
    public Coroutine GrowingPlant { get; private set; }
    public bool IsWaterNeed { get; private set; }
    public Seed Seed { get; private set; }
    public int GrowthStage { get; private set; }

    [SerializeField] private int _minHarvestCount = 1;
    [SerializeField] private int _maxHarvestCount = 3;
    [SerializeField] private float _waterNeedChance = 0.1f;
    [SerializeField] private int _minTimeToDryOut = 5;
    [SerializeField] private int _maxTimeToDryOut = 7;

    private Soil _soil;
    private SeedData _seedData;
    private SoilVisual _soilVisual;
    private Coroutine _dryOutRoutine;
    // private GameSceneManager _gameSceneManager;

    //[Inject]
    //public void Construct(GameSceneManager gameSceneManager)
    //{
    //    _gameSceneManager = gameSceneManager;
    //    SubscribeToEvents();
    //}

    //private void SubscribeToEvents()
    //{
    //    _gameSceneManager.OnHouseUnloading += EndGrowPlant;
    //}

    private void Awake()
    {
        _soil = GetComponent<Soil>();
        _soilVisual = GetComponent<SoilVisual>();
    }

    public void PlantSeed(Seed seed, int growthStage)
    {
        Seed = seed;
        GrowthStage = growthStage;
        _seedData = Seed.Data;
        GrowingPlant = StartCoroutine(GrowPlantRoutine());

        _soil.DisableInteractive();
        _soil.StopStageReset();
    }

    private IEnumerator GrowPlantRoutine()
    {
        while (GrowthStage < _seedData.GrowthStageSprites.Count)
        {
            var stage = _seedData.GrowthStageSprites[GrowthStage];
            _soilVisual.UpdateGrowPlantStage(stage);
            MaybeRequireWater();

            yield return new WaitUntil(() => !IsWaterNeed);
            yield return new WaitForSeconds(Random.Range(_seedData.MinStageTime, _seedData.MaxStageTime));
            GrowthStage++;
        }
        yield return new WaitForSeconds(Random.Range(_seedData.MinStageTime, _seedData.MaxStageTime));
        GrowthStage = 0;
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
        if (GrowingPlant != null)
        {
            StopCoroutine(GrowingPlant);
            GrowingPlant = null;
        }

        _soil.EnableInteractive();
        _soil.StartStageReset();
        _soilVisual.ClearContentPlace();
        IsWaterNeed = false;

        if (_seedData != null)
            SpawnHarvest();
    }

    private void SpawnHarvest()
    {
        for (int i = 0; i < Random.Range(_minHarvestCount, _maxHarvestCount + 1); i++)
            Instantiate(_seedData.IngredientPrefab, transform.position, transform.rotation);
        _seedData = null;
        Seed = null;
    }

    //private void OnDisable()
    //{
    //    _gameSceneManager.OnHouseUnloading -= EndGrowPlant;
    //}
}
