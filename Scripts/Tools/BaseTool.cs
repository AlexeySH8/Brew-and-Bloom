using UnityEngine;

public abstract class BaseTool : BaseHoldItem
{
    [field: SerializeField] public LayerMask InteractionMask { get; private set; }
    [field: SerializeField] public float InteractionDistance { get; private set; }

    public override void Use(Collider2D target) => UseTool(target);

    protected abstract void UseTool(Collider2D target);

    public override void Discard() { }
}
