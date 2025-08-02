using System;
using UnityEngine;

public class SoilVisual : MonoBehaviour
{
    [SerializeField] private Sprite[] _stageSprites;

    private SpriteRenderer _�ontentSprite;
    private SpriteRenderer _soilSprite;

    public void Init()
    {
        _soilSprite = GetComponent<SpriteRenderer>();
        _�ontentSprite = transform.GetChild(0).GetComponentInChildren<SpriteRenderer>();
    }

    public void UpdateSprite(int currentStage)
    {
        if (currentStage >= (int)CultivationStage.Soil)
        {
            _soilSprite.sprite = _stageSprites[currentStage];
            _�ontentSprite.gameObject.SetActive(false);
        }
        else
        {
            _soilSprite.sprite = _stageSprites[(int)CultivationStage.Soil];
            _�ontentSprite.sprite = _stageSprites[currentStage];
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
