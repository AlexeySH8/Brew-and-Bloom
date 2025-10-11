using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shovel : BaseUsableItem
{
    public override bool TryUse()
    {
        Collider2D target = DetectTarget();
        if (target != null &&
            target.TryGetComponent(out IShovelTarget shovelTarget))
        {
            shovelTarget.InteractWithShovel();
            return true;
        }
        return false;
    }
}
