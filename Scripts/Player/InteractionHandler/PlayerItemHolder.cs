using UnityEngine;

public class PlayerItemHolder : BaseItemHolder
{
    [SerializeField] private Transform _holdToolPoint;
    [SerializeField] private Transform _holdItemPoint;
    [SerializeField] private float _throwingForce;

    private PlayerVisual _witchVisual;
    private Rigidbody2D _heldItemRB;

    public override Transform ParentPoint => _witchVisual.transform;
    public override int SortingOrderOffset => _witchVisual.SpriteRenderer.sortingOrder + 1;

    private void Awake()
    {
        _witchVisual = GetComponentInChildren<PlayerVisual>();
    }

    public void PickUp(BaseHoldItem holdItem)
    {
        if (TryReceive(holdItem))
            SetItemPosition();
    }

    public void Drop(Vector2 forceVector)
    {
        BaseHoldItem heldItem = GiveItem();

        if (heldItem == null) return;

        var rb = heldItem.Rigidbody;
        rb.transform.position += (Vector3)(forceVector * 0.2f); // Moves the object collider away from the player collider
        rb.AddForce(forceVector * _throwingForce, ForceMode2D.Impulse);
    }

    private void SetItemPosition()
    {
        if (_heldItem.TryGetComponent(out BaseUsableItem tool))
            _heldItem.transform.position = _holdToolPoint.position;
        else
            _heldItem.transform.position = _holdItemPoint.position;
    }

    public GameObject GetHeldItem() => _heldItem?.gameObject;
}
