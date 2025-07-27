using UnityEngine;

public interface IPlayerInput
{
    float GetHorizontal();
    float GetVertical();
    bool IsInteractPressed();
    public bool IsDropPressed();
}
