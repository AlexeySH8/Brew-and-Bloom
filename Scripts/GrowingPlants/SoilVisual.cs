using System;
using UnityEngine;

public class SoilVisual : MonoBehaviour
{
    [SerializeField] private Sprite[] _stageSprites;

    private Soil _soil;
    private SpriteRenderer _�ontentSprite;
    private SpriteRenderer _soilSprite;

    private void Awake()
    {
        _soil = GetComponent<Soil>();
        _soilSprite = GetComponent<SpriteRenderer>();
        _�ontentSprite = transform.GetChild(0).GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        UpdateSoilVisual();
    }

    public void UpdateSoilVisual()
    {
        if (_soil.Stage < CultivationStage.Soil)
        {
            _soilSprite.sprite = _stageSprites[(int)CultivationStage.Soil];
            _�ontentSprite.sprite = _stageSprites[(int)_soil.Stage];
        }
        else
        {
            _soilSprite.sprite = _stageSprites[(int)_soil.Stage];
            _�ontentSprite.gameObject.SetActive(false);
        }
    }

    private void OnValidate()
    {
        int expectedLength = Enum.GetValues(typeof(CultivationStage)).Length;

        if (_stageSprites == null || _stageSprites.Length != expectedLength)
        {
            _stageSprites = new Sprite[expectedLength];
        }
    }
}
