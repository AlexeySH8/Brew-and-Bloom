using UnityEngine;

public class Soil : MonoBehaviour, IPickTarget, IShovelTarget, IStaffTarget, IReceiveHeldItem
{
    [SerializeField] private CultivationStage _currentStage;

    private GrowPlant _growPlant;
    private SoilVisual _soilVisual;

    private const string PickTargetLayer = "PickTarget";
    private const string ShovelTargetLayer = "ShovelTarget";
    private const string StaffTargetLayer = "StaffTarget";
    private const string InteractiveItemtLayer = "InteractiveItem";
    private const string DefaultLayer = "Default";

    private void Awake()
    {
        _soilVisual = GetComponent<SoilVisual>();
        _growPlant = GetComponent<GrowPlant>();
    }

    private void Start()
    {
        UpdateLayer();
        _soilVisual.UpdateCultivationStage(_currentStage);
    }

    public void InteractWithPick() => Cultivate();

    public void InteractWithShovel() => Cultivate();

    public void InteractWithStaff() => _growPlant.SetWateredPlant(false);

    public bool TryReceive(BaseHoldItem heldItem)
    {
        if (heldItem.TryGetComponent(out Seed seed))
        {
            DisableToInteractive();
            _growPlant.PlantSeed(seed.Data);
            heldItem.Discard();
            return true;
        }
        return false;
    }

    private void Cultivate()
    {
        if (_currentStage == CultivationStage.CultivatedSoil) return;
        _currentStage++;
        UpdateLayer();
        _soilVisual.UpdateCultivationStage(_currentStage);
    }

    public void UpdateLayer()
    {
        if (_currentStage < CultivationStage.Soil)
            gameObject.layer = LayerMask.NameToLayer(PickTargetLayer);
        else if (_currentStage < CultivationStage.CultivatedSoil)
            gameObject.layer = LayerMask.NameToLayer(ShovelTargetLayer);
        else if (_growPlant.IsWaterNeed)
            gameObject.layer = LayerMask.NameToLayer(StaffTargetLayer);
        else
            EnableToInteractive();
    }

    private void EnableToInteractive() =>
        gameObject.layer = LayerMask.NameToLayer(InteractiveItemtLayer);

    private void DisableToInteractive() =>
        gameObject.layer = LayerMask.NameToLayer(DefaultLayer);
}
