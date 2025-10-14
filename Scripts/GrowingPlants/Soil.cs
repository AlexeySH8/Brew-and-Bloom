using System.Collections;
using UnityEngine;

public enum CultivationStage
{
    BigStone,
    MediumStone,
    SmallStone,
    Soil,
    LooseSoil,
    CultivatedSoil
}

public class Soil : MonoBehaviour, IPickTarget, IShovelTarget, IReceiveHeldItem
{
    [field: SerializeField] public CultivationStage Stage { get; private set; }

    [SerializeField] private int _minHarvestCount = 1;
    [SerializeField] private int _maxHarvestCount = 3;

    private SoilVisual _soilVisual;

    private void Awake()
    {
        _soilVisual = GetComponent<SoilVisual>();
        UpdateLayer();
    }

    public void InteractWithPick() => Cultivate();

    public void InteractWithShovel() => Cultivate();

    public bool TryReceive(BaseHoldItem heldItem)
    {
        if (heldItem.TryGetComponent(out Seed seed))
        {
            StartCoroutine(PlantsSeed(seed.Data));
            heldItem.Discard();
            return true;
        }
        return false;
    }

    private IEnumerator PlantsSeed(SeedData seedData)
    {
        if (!seedData)
            Debug.LogError($"{gameObject.name} has no SeedData");

        DisableToInteractive();
        yield return StartCoroutine(GrowPlant(seedData));
        EnableToInteractive();
    }

    private IEnumerator GrowPlant(SeedData seedData)
    {
        for (int i = 0; i < seedData.GrowthStageSprites.Count; i++)
        {
            Sprite stage = seedData.GrowthStageSprites[i];
            _soilVisual.UpdateGrowPlantStage(stage);
            yield return new WaitForSeconds(Random.Range(seedData.MinStageTime, seedData.MaxStageTime));
        }

        SpawnHarvest(seedData);
        _soilVisual.ClearContentPlace();
    }

    private void SpawnHarvest(SeedData seedData)
    {
        for (int i = 0; i < Random.Range(_minHarvestCount, _maxHarvestCount); i++)
        {
            Instantiate(seedData.IngredientPrefab, transform.position, transform.rotation);
        }
    }

    private void Cultivate()
    {
        if (Stage == CultivationStage.CultivatedSoil) return;
        Stage++;
        UpdateLayer();
        _soilVisual.UpdateCultivationSoilStage();
    }

    private void UpdateLayer()
    {
        if (Stage < CultivationStage.Soil)
            gameObject.layer = LayerMask.NameToLayer("PickTarget");
        else if (Stage < CultivationStage.CultivatedSoil)
            gameObject.layer = LayerMask.NameToLayer("ShovelTarget");
        else
            EnableToInteractive();
    }

    private void EnableToInteractive() => gameObject.layer = LayerMask.NameToLayer("InteractiveItem");

    private void DisableToInteractive() => gameObject.layer = LayerMask.NameToLayer("Default");
}
