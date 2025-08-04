using UnityEngine;

public class Soil : MonoBehaviour, IPickTarget, IShovelTarget
{
    public CultivationStage Stage { get; private set; }
    [SerializeField] private CultivationStage _stage = CultivationStage.BigStone;

    private SoilVisual _visual;

    private void Awake()
    {
        Stage = _stage;
        _visual = GetComponent<SoilVisual>();
        UpdateLayer();
    }

    public void InteractWithPick()
    {
        Cultivate();
    }

    public void InteractWithShovel()
    {
        Cultivate();
    }

    private void Cultivate()
    {
        if (Stage == CultivationStage.CultivatedSoil) return;
        Stage++;
        UpdateLayer();
        _visual.UpdateSoilVisual();
    }

    private void UpdateLayer()
    {
        if (Stage < CultivationStage.Soil)
            gameObject.layer = LayerMask.NameToLayer("PickTarget");
        else if (Stage < CultivationStage.CultivatedSoil)
            gameObject.layer = LayerMask.NameToLayer("ShovelTarget");
        else
            gameObject.layer = LayerMask.NameToLayer("Default");
    }
}
