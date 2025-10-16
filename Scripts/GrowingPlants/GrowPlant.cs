using System.Collections;
using UnityEngine;

public class GrowPlant : MonoBehaviour
{
    public bool IsWaterNeed { get; private set; }

    [SerializeField] private int _minHarvestCount = 1;
    [SerializeField] private int _maxHarvestCount = 3;

    private Soil _soil;
    private SoilVisual _soilVisual;

    private void Awake()
    {
        _soil = GetComponent<Soil>();
        _soilVisual = GetComponent<SoilVisual>();
    }

    public void PlantSeed(SeedData seedData)
    {
        if (!seedData)
        {
            Debug.LogError($"{gameObject.name} has no SeedData");
            return;
        }
        StartCoroutine(StartGrowPlant(seedData));
    }

    private IEnumerator StartGrowPlant(SeedData seedData)
    {
        for (int i = 0; i < seedData.GrowthStageSprites.Count; i++)
        {
            Sprite stage = seedData.GrowthStageSprites[i];
            _soilVisual.UpdateGrowPlantStage(stage);

            PlantNeedWater();
            yield return new WaitUntil(() => !IsWaterNeed);

            yield return new WaitForSeconds(Random.Range(seedData.MinStageTime, seedData.MaxStageTime));
        }
        EndGrowPlant(seedData);
    }

    private void PlantNeedWater()
    {
        //bool isNeed = Random.value > 0.2f;
        bool isNeed = true;

        if (isNeed)
            SetWateredPlant(true);
    }

    public void SetWateredPlant(bool isWaterNeed)
    {
        IsWaterNeed = isWaterNeed;
        _soilVisual.SetWaterNeedIcon(IsWaterNeed);
        _soil.UpdateLayer();
    }

    private void EndGrowPlant(SeedData seedData)
    {
        if (seedData != null)
            SpawnHarvest(seedData);
        _soil.UpdateLayer();
        _soilVisual.ClearContentPlace();
    }

    private void SpawnHarvest(SeedData seedData)
    {
        for (int i = 0; i < Random.Range(_minHarvestCount, _maxHarvestCount); i++)
            Instantiate(seedData.IngredientPrefab, transform.position, transform.rotation);
    }
}
