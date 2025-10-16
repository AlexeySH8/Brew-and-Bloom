using System;
using UnityEngine;

public class SoilVisual : MonoBehaviour
{
    [SerializeField] private GameObject _waterNeedIcon;
    [SerializeField] private Sprite[] _stageSprites;

    private SpriteRenderer _�ontentSprite;
    private SpriteRenderer _soilSprite;

    private void Awake()
    {
        _soilSprite = GetComponent<SpriteRenderer>();
        _�ontentSprite = transform.GetChild(0).GetComponentInChildren<SpriteRenderer>();
    }

    public void UpdateCultivationStage(CultivationStage currentStage)
    {
        if (currentStage < CultivationStage.Soil)
        {
            _soilSprite.sprite = _stageSprites[(int)CultivationStage.Soil];
            _�ontentSprite.sprite = _stageSprites[(int)currentStage];
        }
        else
        {
            _soilSprite.sprite = _stageSprites[(int)currentStage];
            ClearContentPlace();
        }
    }

    public void UpdateGrowPlantStage(Sprite stage)
    {
        _�ontentSprite.sprite = stage;
    }

    public void ClearContentPlace() => _�ontentSprite.sprite = null;

    public void SetWaterNeedIcon(bool isNeedWater) => _waterNeedIcon.SetActive(isNeedWater);

    private void OnValidate()
    {
        int expectedLength = Enum.GetValues(typeof(CultivationStage)).Length;

        if (_stageSprites == null || _stageSprites.Length != expectedLength)
        {
            _stageSprites = new Sprite[expectedLength];
        }
    }
}
