using UnityEngine;

public class Shovel : BaseUsableItem
{
    public override bool TryUse()
    {
        Collider2D target = DetectTarget();
        if (target != null &&
            target.TryGetComponent(out IShovelTarget shovelTarget))
        {
            SFX.Instance.PlayChop();
            shovelTarget.InteractWithShovel();
            return true;
        }
        return false;
    }
}
