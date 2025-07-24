using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCInput : IPlayerInput
{
    float IPlayerInput.GetHorizontal() => Input.GetAxisRaw("Horizontal");
    float IPlayerInput.GetVertical() => Input.GetAxisRaw("Vertical");
}
