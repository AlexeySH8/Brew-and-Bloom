using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shovel : BaseTool
{
    protected override void UseTool(Collider2D target)
    {
        if (target.TryGetComponent(out IShovelTarget pickTarget))
            pickTarget.InteractWithShovel();
    }
}
