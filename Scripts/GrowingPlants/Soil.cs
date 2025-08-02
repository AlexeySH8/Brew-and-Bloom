using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soil : MonoBehaviour, IPickTarget, IShovelTarget
{
    [SerializeField] private CultivationStage _currentStage = CultivationStage.BigStone;

    private SoilVisual _visual;

    private void Awake()
    {
        _visual = GetComponent<SoilVisual>();
        _visual.Init();
        _visual.UpdateSprite((int)_currentStage);
    }

    public void InteractWithPick()
    {
        if (_currentStage < CultivationStage.Soil)
            Cultivate();
    }

    public void InteractWithShovel()
    {
        if (_currentStage > CultivationStage.SmallStone && _currentStage < CultivationStage.CultivatedSoil)
            Cultivate();
    }

    private void Cultivate()
    {
        _currentStage++;
        _visual.UpdateSprite((int)_currentStage);
    }
}
