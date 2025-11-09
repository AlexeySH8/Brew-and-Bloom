using UnityEngine;

public class Pick : BaseUsableItem
{
    public override bool TryUse()
    {
        Collider2D target = DetectTarget();
        if (target != null &&
            target.TryGetComponent(out IPickTarget pickTarget))
        {
            SFX.Instance.PlayMine();
            pickTarget.InteractWithPick();
            return true;
        }
        return false;
    }
}
