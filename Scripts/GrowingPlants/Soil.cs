using System.Collections;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Zenject;

public class Soil : BaseItemHolder, IPickTarget, IShovelTarget, IStaffTarget, IDataPersistence
{
    [SerializeField] private CultivationStage _currentStage;
    [SerializeField] private float _minResetStageTime;
    [SerializeField] private float _maxResetStageTime;
    [SerializeField] private string _soilId;

    private GrowPlant _growPlant;
    private SeedSaveData _seedSaveData;
    private SoilVisual _soilVisual;
    private Coroutine _stageResetRoutine;
    private ItemStandManager _itemStandManager;
    private IDataPersistenceManager _persistenceManager;

    private const string PickTargetLayer = "PickTarget";
    private const string ShovelTargetLayer = "ShovelTarget";
    private const string StaffTargetLayer = "StaffTarget";
    private const string InteractiveItemLayer = "InteractiveItem";
    private const string DefaultLayer = "Default";

    public override Transform ParentPoint => _soilVisual.SpawnHarvestPos;

    public override int SortingOrderOffset => _soilVisual.SortingOrder;

    [Inject]
    public void Construct(IDataPersistenceManager persistenceManager,
        ItemStandManager itemStandManager)
    {
        _itemStandManager = itemStandManager;
        _persistenceManager = persistenceManager;
        _persistenceManager.Register(this);
    }

    protected override void Awake()
    {
        _holderId = _soilId;
        _soilVisual = GetComponent<SoilVisual>();
        _growPlant = GetComponent<GrowPlant>();
        base.Awake();
    }

    private void Start()
    {
        UpdateStage();
        if (_seedSaveData != null &&
            !string.IsNullOrEmpty(_seedSaveData.SeedPrefabPath))
        {
            var seedPref = Resources.Load<Seed>(_seedSaveData.SeedPrefabPath);
            int growthStageIndex = _seedSaveData.growthStageIndex;
            _growPlant.PlantSeed(seedPref.Data, _seedSaveData.SeedPrefabPath, growthStageIndex);
        }
    }

    public void InteractWithPick() => Cultivate();

    public void InteractWithShovel() => Cultivate();

    public void InteractWithStaff() => _growPlant.SetPlantNeedWater(false);

    public override bool TryReceive(BaseHoldItem heldItem) => TryPlantSeed(heldItem);

    private bool TryPlantSeed(BaseHoldItem item)
    {
        if (_growPlant.GrowingPlant == null &&
            _heldItem == null &&
            item.TryGetComponent(out Seed seed))
        {
            _growPlant.PlantSeed(seed.Data, seed.PrefabPath, 0);
            item.Discard();
            return true;
        }
        return false;
    }

    public override BaseHoldItem GiveItem()
    {
        BaseHoldItem harvest = base.GiveItem();
        if (harvest == null) return null;

        StartStageReset();
        if (_itemStandManager.TryPlaceHarvest(harvest))
            return null;
        else
            return harvest;
    }

    private void Cultivate()
    {
        if (_currentStage >= CultivationStage.CultivatedSoil) return;

        _currentStage = (CultivationStage)((int)_currentStage + 1);
        UpdateStage();
    }

    public void StartStageReset()
    {
        StopStageReset();
        _stageResetRoutine = StartCoroutine(StageResetRoutine());
    }

    public void StopStageReset()
    {
        if (_stageResetRoutine != null)
        {
            StopCoroutine(_stageResetRoutine);
            _stageResetRoutine = null;
        }
    }

    private IEnumerator StageResetRoutine()
    {
        yield return new WaitForSeconds(
            Random.Range(_minResetStageTime, _maxResetStageTime));

        _currentStage = CultivationStage.BigStone;
        UpdateStage();
    }

    public void UpdateLayer()
    {
        if (_currentStage < CultivationStage.Soil)
            gameObject.layer = LayerMask.NameToLayer(PickTargetLayer);
        else if (_currentStage < CultivationStage.CultivatedSoil)
            gameObject.layer = LayerMask.NameToLayer(ShovelTargetLayer);
        else
            EnableInteractive();
    }

    private void UpdateStage()
    {
        StartStageReset();
        UpdateLayer();
        _soilVisual.UpdateCultivationStage(_currentStage);
    }

    public void EnableInteractiveWithStaff() =>
        gameObject.layer = LayerMask.NameToLayer(StaffTargetLayer);

    public void EnableInteractive() =>
        gameObject.layer = LayerMask.NameToLayer(InteractiveItemLayer);

    public void DisableInteractive() =>
        gameObject.layer = LayerMask.NameToLayer(DefaultLayer);

    public void LoadData(GameData gameData)
    {
        SoilSaveData soilData = gameData.SoilsSaveData
            .FirstOrDefault(s => s.SoilId == _soilId);

        if (soilData == null) return;

        _currentStage = (CultivationStage)soilData.CultivationStage;
        _seedSaveData = soilData.SeedSaveData;

        LoadHeldItem(gameData);
    }

    public void SaveData(GameData gameData)
    {
        SoilSaveData soilData = gameData.SoilsSaveData.
            FirstOrDefault(s => s.SoilId == _soilId);

        if (soilData == null)
        {
            soilData = new SoilSaveData() { SoilId = _soilId };
            gameData.SoilsSaveData.Add(soilData);
        }
        soilData.CultivationStage = (int)_currentStage;

        SeedSaveData seedSaveData = new SeedSaveData();
        seedSaveData.SeedPrefabPath = _growPlant.PrefabPath;
        seedSaveData.growthStageIndex = _growPlant.GrowthStage;
        soilData.SeedSaveData = seedSaveData;

        SaveHeldItem(gameData);
    }

    private void OnDestroy()
    {
        _persistenceManager.Unregister(this);
    }

    [ContextMenu("Generate Unique Id")]
    private void GenerateUniqueId()
    {
        _soilId = GUID.Generate().ToString();
        EditorUtility.SetDirty(this);
    }
}
