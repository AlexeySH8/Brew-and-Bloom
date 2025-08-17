using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : BaseTool
{
    protected override void UseTool(Collider2D target)
    {
        Debug.Log("Staff Use!");
    }
}
