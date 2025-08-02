using System;
using UnityEngine;

public class SoilVisual : MonoBehaviour
{
    [SerializeField] private Sprite[] _stageSprites;

    private SpriteRenderer _ñontentSprite;
    private SpriteRenderer _soilSprite;

    public void Init()
    {
        _soilSprite = GetComponent<SpriteRenderer>();
        _ñontentSprite = transform.GetChild(0).GetComponentInChildren<SpriteRenderer>();
    }

    public void UpdateSprite(int currentStage)
    {
        if (currentStage >= (int)CultivationStage.Soil)
        {
            _soilSprite.sprite = _stageSprites[currentStage];
            _ñontentSprite.gameObject.SetActive(false);
        }
        else
        {
            _soilSprite.sprite = _stageSprites[(int)CultivationStage.Soil];
            _ñontentSprite.sprite = _stageSprites[currentStage];
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
