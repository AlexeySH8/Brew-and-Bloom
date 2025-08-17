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

public class Soil : MonoBehaviour, IPickTarget, IShovelTarget, IReceivesHeldItem
{
    [field: SerializeField] public CultivationStage Stage { get; private set; }

    private SoilVisual _soilVisual;
    private GrowPlant _growPlant;

    private void Awake()
    {
        _soilVisual = GetComponent<SoilVisual>();
        _growPlant = GetComponent<GrowPlant>();
        UpdateLayer();
    }

    public void InteractWithPick() => Cultivate();

    public void InteractWithShovel() => Cultivate();

    public void Receive(GameObject heldItem) => PlantsSeed(heldItem);

    private void PlantsSeed(GameObject heldItem)
    {
        if (heldItem.TryGetComponent(out Seed seed) && _growPlant.GrowingPlant == null)
        {
            if (!seed.Data)
                Debug.LogError($"{gameObject.name} has no SeedData");

            heldItem.GetComponent<BaseHoldItem>().Discard();
            _growPlant.PlantSeed(seed.Data, _soilVisual);
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
            gameObject.layer = LayerMask.NameToLayer("InteractiveItem");
    }
}
