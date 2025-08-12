using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowPlant : MonoBehaviour
{
    public Coroutine GrowingPlant { get; private set; }

    //private Coroutine _givingHarvest;
   // private float _minTimeToNextHarvest = 1f;
    //private float _maxTimeToNextHarvest = 3f;
    private int _minHarvestCount = 1;
    private int _maxHarvestCount = 3;

    public void PlantSeed(SeedData seedData, SoilVisual soilVisual)
    {
        GrowingPlant = StartCoroutine(GrowPlantCoroutine(seedData, soilVisual));
    }

    private IEnumerator GrowPlantCoroutine(SeedData seedData, SoilVisual soilVisual)
    {
        for (int i = 0; i < seedData.GrowthStageSprites.Count; i++)
        {
            Sprite stage = seedData.GrowthStageSprites[i];
            soilVisual.UpdateGrowPlantStage(stage);
            yield return new WaitForSeconds(Random.Range(seedData.MinStageTime, seedData.MaxStageTime));
        }

        //yield return _givingHarvest = StartCoroutine(
        //    GiveHarvest(seedData.IngredientPrefab, Random.Range(_minHarvestCount, _maxHarvestCount)));
        for (int i = 0; i < Random.Range(_minHarvestCount, _maxHarvestCount); i++)
        {
            Instantiate(seedData.IngredientPrefab, transform.position, transform.rotation);
        }
        soilVisual.ClearContentPlace();
        GrowingPlant = null;
       // _givingHarvest = null;
    }

    //private IEnumerator GiveHarvest(GameObject harvest, int harvestCount)
    //{
    //    for (int i = 0; i < harvestCount; i++)
    //    {
    //        Instantiate(harvest, transform.position, transform.rotation);
    //    }
    //}
}
