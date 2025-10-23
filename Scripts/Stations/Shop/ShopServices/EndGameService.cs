using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameService : IShopService
{
    public bool TryApply()
    {
        Debug.Log("Game is End");
        return true;
    }
}
