using System;
using UnityEditor.SceneManagement;
using UnityEngine;

public class SoilVisual : MonoBehaviour
{
    [SerializeField] private GameObject _waterNeedIcon;
    [SerializeField] private Sprite[] _stageSprites;

    private SpriteRenderer _ñontentSprite;
    private SpriteRenderer _soilSprite;

    public Transform SpawnHarvestPos => _ñontentSprite.gameObject.transform;
    public int SortingOrder => _ñontentSprite.sortingOrder;

    private void Awake()
    {
        _soilSprite = GetComponent<SpriteRenderer>();
        _ñontentSprite = transform.GetChild(0).GetComponentInChildren<SpriteRenderer>();
    }

    public void UpdateCultivationStage(CultivationStage currentStage)
    {
        if (currentStage < CultivationStage.Soil)
        {
            _soilSprite.sprite = _stageSprites[(int)CultivationStage.Soil];
            _ñontentSprite.sprite = _stageSprites[(int)currentStage];
        }
        else
        {
            _soilSprite.sprite = _stageSprites[(int)currentStage];
            ClearContentPlace();
        }
    }

    public void UpdateGrowPlantStage(Sprite stage)
    {
        _ñontentSprite.sprite = stage;
    }

    public void ClearContentPlace()
    {
        _ñontentSprite.sprite = null;
        SetWaterNeedIcon(false);
    }

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
