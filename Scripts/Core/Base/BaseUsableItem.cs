using UnityEngine;

public abstract class BaseUsableItem : BaseHoldItem
{
    [field: SerializeField] public LayerMask InteractionMask { get; private set; }
    [field: SerializeField] public float InteractionDistance { get; private set; }

    public abstract bool TryUse();

    protected virtual Collider2D DetectTarget()
    {
        float itemFacing = Mathf.Sign(_currentHolder.ParentPoint.localScale.x);
        Vector2 direction = new Vector2(itemFacing, 0);
        Vector2 origin = transform.position;
        RaycastHit2D toolTarget = Physics2D.Raycast(origin, direction,
            InteractionDistance, InteractionMask);
        Debug.DrawRay(origin, direction * InteractionDistance, Color.red, 0.5f);
        return toolTarget.collider;
    }
}
