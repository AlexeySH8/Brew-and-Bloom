using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pick : BaseTool
{
    public override void Use(float faceDirection)
    {
        Collider2D interactiveItem = Detect(faceDirection);
        if (interactiveItem)
        {
            if (interactiveItem.TryGetComponent(out IPickTarget pickTarget))
                pickTarget.InteractWithPick();
        }
    }
}
