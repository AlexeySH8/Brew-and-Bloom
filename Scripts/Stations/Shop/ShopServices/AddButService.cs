using UnityEngine;

public class AddButService : IShopService
{
    private ButPen _butPen;

    public AddButService(ButPen butPen)
    {
        _butPen = butPen;
    }

    public bool TryApply()
    {
        return _butPen.TrySpawnBut();
    }
}

