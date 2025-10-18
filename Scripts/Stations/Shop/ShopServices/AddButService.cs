using UnityEngine;

public class AddButService : MonoBehaviour, IShopService
{
    private ButPen _butPen;

    public bool TryApply()
    {
        _butPen = FindAnyObjectByType<ButPen>();
        return _butPen.TrySpawnBut();
    }
}

