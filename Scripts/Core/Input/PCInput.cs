using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCInput : IPlayerInput
{
    public float GetHorizontal() => Input.GetAxisRaw("Horizontal");
    public float GetVertical() => Input.GetAxisRaw("Vertical");
    public bool IsInteractPressed() => Input.GetKeyDown(KeyCode.E);
    public bool IsDropPressed() => Input.GetKeyDown(KeyCode.Space);
}
