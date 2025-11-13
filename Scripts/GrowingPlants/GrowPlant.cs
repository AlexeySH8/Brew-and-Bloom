using System.Collections;
using UnityEngine;
using Zenject;

public class GrowPlant : MonoBehaviour
{
    public Coroutine GrowingPlant { get; private set; }
    public bool IsWaterNeed { get; private set; }
    public int GrowthStage { get; private set; }
    public string PrefabPath { get; private set; } = string.Empty;

    private float _waterNeedChance = 0.2f;
    private int _minTimeToDryOut = 10;
    private int _maxTimeToDryOut = 30;

    private Soil _soil;
    private SeedData _seedData;
    private SoilVisual _soilVisual;
    private Coroutine _dryOutRoutine;

    private void Awake()
    {
        _soil = GetComponent<Soil>();
        _soilVisual = GetComponent<SoilVisual>();
    }

    public void PlantSeed(SeedData seedData, string prefabPath, int growthStage)
    {
        SFX.Instance.PlayPlantSeed();
        _soil.DisableInteractive();
        _soil.StopStageReset();
        GrowthStage = growthStage;
        _seedData = seedData;
        PrefabPath = prefabPath;
        GrowingPlant = StartCoroutine(GrowPlantRoutine());
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
            SFX.Instance.PlayPlantNeedWater();
            _soil.EnableInteractiveWithStaff();
            SetPlantNeedWater(isWaterNeed);
            _dryOutRoutine = StartCoroutine(PlantDriedOut());
        }
    }

    private IEnumerator PlantDriedOut()
    {
        yield return new WaitForSeconds(
            Random.Range(_minTimeToDryOut, _maxTimeToDryOut));
        _seedData = null;
        SFX.Instance.PlayPlantDriedOut();
        EndGrowPlant();
    }

    public void SetPlantNeedWater(bool isWaterNeed)
    {
        if (IsWaterNeed == isWaterNeed) return;

        if (!isWaterNeed && _dryOutRoutine != null)
        {
            SFX.Instance.PlayWaterMagic();
            StopCoroutine(_dryOutRoutine);
            _dryOutRoutine = null;
            _soil.DisableInteractive();
        }

        IsWaterNeed = isWaterNeed;
        _soilVisual.SetWaterNeedIcon(IsWaterNeed);
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
        PrefabPath = string.Empty;

        if (_seedData != null)
            SpawnHarvest();
    }

    private void SpawnHarvest()
    {
        SFX.Instance.PlayPlantGetHarvest();
        int min = _seedData.MinHarvestCount;
        int max = _seedData.MaxHarvestCount;
        for (int i = 0; i < Random.Range(min, max); i++)
            Instantiate(_seedData.IngredientPrefab, transform.position, transform.rotation);
        _seedData = null;
    }
}
