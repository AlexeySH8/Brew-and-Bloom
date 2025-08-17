using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pick : BaseTool
{
    protected override void UseTool(Collider2D target)
    {
        if (target.TryGetComponent(out IPickTarget pickTarget))
            pickTarget.InteractWithPick();
    }
}
