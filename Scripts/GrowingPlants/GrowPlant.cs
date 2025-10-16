using System.Collections;
using UnityEngine;

public class GrowPlant : MonoBehaviour
{
    public bool IsWaterNeed { get; private set; }

    [SerializeField] private int _minHarvestCount = 1;
    [SerializeField] private int _maxHarvestCount = 3;
    [SerializeField] private int _minTimeToDryOut = 5;
    [SerializeField] private int _maxTimeToDryOut = 7;

    private Soil _soil;
    private SoilVisual _soilVisual;
    private Coroutine _growingPlant;
    private Coroutine _dryOutRoutine;

    private void Awake()
    {
        _soil = GetComponent<Soil>();
        _soilVisual = GetComponent<SoilVisual>();
    }

    public void PlantSeed(SeedData seedData)
    {
        if (seedData == null || _growingPlant != null) return;
        _growingPlant = StartCoroutine(StartGrowPlant(seedData));
    }

    private IEnumerator StartGrowPlant(SeedData seedData)
    {
        foreach (Sprite stage in seedData.GrowthStageSprites)
        {
            _soilVisual.UpdateGrowPlantStage(stage);
            MaybeRequireWater();
            yield return new WaitUntil(() => !IsWaterNeed);
            yield return new WaitForSeconds(Random.Range(seedData.MinStageTime, seedData.MaxStageTime));
        }
        EndGrowPlant(seedData);
    }

    private void MaybeRequireWater()
    {
        bool isWaterNeed = Random.value > 0.8f; // 20% true

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
        EndGrowPlant(null);
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

    private void EndGrowPlant(SeedData seedData)
    {
        if (seedData != null)
            SpawnHarvest(seedData);

        if (_growingPlant != null)
        {
            StopCoroutine(_growingPlant);
            _growingPlant = null;
        }

        IsWaterNeed = false;

        _soil.UpdateLayer();
        _soilVisual.ClearContentPlace();
    }

    private void SpawnHarvest(SeedData seedData)
    {
        for (int i = 0; i < Random.Range(_minHarvestCount, _maxHarvestCount + 1); i++)
            Instantiate(seedData.IngredientPrefab, transform.position, transform.rotation);
    }
}
