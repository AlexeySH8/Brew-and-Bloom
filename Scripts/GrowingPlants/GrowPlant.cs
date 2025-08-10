using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowPlant : MonoBehaviour
{
    private Coroutine _growPlant;

    public void PlantSeed(SeedData seedData, SoilVisual soilVisual)
    {
        _growPlant = StartCoroutine(GrowPlantCoroutine(seedData, soilVisual));
    }

    private IEnumerator GrowPlantCoroutine(SeedData seedData, SoilVisual soilVisual)
    {
        for (int i = 0; i < seedData.GrowthStageSprites.Count; i++)
        {
            Sprite stage = seedData.GrowthStageSprites[i];
            soilVisual.UpdateGrowPlantStage(stage);
            yield return new WaitForSeconds(Random.Range(seedData.MinStageTime, seedData.MaxStageTime));
        }
        soilVisual.ClearContentPlace();
        Instantiate(seedData.IngredientPrefab, transform.position, transform.rotation);
    }
}
