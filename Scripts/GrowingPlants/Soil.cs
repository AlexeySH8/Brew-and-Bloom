using System.Collections;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Zenject;

public class Soil : MonoBehaviour, IPickTarget, IShovelTarget, IStaffTarget, IReceiveHeldItem, IDataPersistence
{
    [SerializeField] private CultivationStage _currentStage;
    [SerializeField] private float _minResetStageTime;
    [SerializeField] private float _maxResetStageTime;
    [SerializeField] private string _soilId;

    private GrowPlant _growPlant;
    private SoilVisual _soilVisual;
    private Coroutine _stageResetRoutine;
    private IDataPersistenceManager _persistenceManager;

    private const string PickTargetLayer = "PickTarget";
    private const string ShovelTargetLayer = "ShovelTarget";
    private const string StaffTargetLayer = "StaffTarget";
    private const string InteractiveItemLayer = "InteractiveItem";
    private const string DefaultLayer = "Default";

    [Inject]
    public void Construct(IDataPersistenceManager persistenceManager)
    {
        _persistenceManager = persistenceManager;
        _persistenceManager.Register(this);
    }

    private void Awake()
    {
        _soilVisual = GetComponent<SoilVisual>();
        _growPlant = GetComponent<GrowPlant>();
    }

    private void Start()
    {
        UpdateStage();
    }

    public void InteractWithPick() => Cultivate();

    public void InteractWithShovel() => Cultivate();

    public void InteractWithStaff() => _growPlant.SetPlantNeedWater(false);

    public bool TryReceive(BaseHoldItem heldItem)
    {
        if (heldItem.TryGetComponent(out Seed seed))
        {
            DisableInteractive();
            StopStageReset();
            _growPlant.PlantSeed(seed.Data);
            heldItem.Discard();
            return true;
        }
        return false;
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

    private void StopStageReset()
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
        else if (_growPlant.IsWaterNeed)
            gameObject.layer = LayerMask.NameToLayer(StaffTargetLayer);
        else
            EnableInteractive();
    }

    private void UpdateStage()
    {
        StartStageReset();
        UpdateLayer();
        _soilVisual.UpdateCultivationStage(_currentStage);
    }

    private void EnableInteractive() =>
        gameObject.layer = LayerMask.NameToLayer(InteractiveItemLayer);

    private void DisableInteractive() =>
        gameObject.layer = LayerMask.NameToLayer(DefaultLayer);

    public void LoadData(GameData gameData)
    {
        SoilSaveData soilData = gameData.SoilsSaveData
            .FirstOrDefault(s => s.SoilId == _soilId);

        if (soilData == null) return;

        _currentStage = (CultivationStage)soilData.CultivationStage;
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
