using UnityEngine;

public class PlayerItemHolder : BaseItemHolder
{
    [SerializeField] private Transform _holdToolPoint;
    [SerializeField] private Transform _holdItemPoint;
    [SerializeField] private float _throwingForce;

    private PlayerVisual _playerVisual;
    private float _dropOffset = 0.2f;
    public override Transform ParentPoint => _playerVisual.transform;
    public override int SortingOrderOffset => _playerVisual.SpriteRenderer.sortingOrder + 1;

    private void Awake()
    {
        _playerVisual = GetComponentInChildren<PlayerVisual>();
    }

    public void PickUp(BaseHoldItem holdItem)
    {
        if (TryReceive(holdItem))
        {
            SFX.Instance.PlayPickUpItem();
            SetItemPosition();
        }
    }

    public void Drop(Vector2 forceDirection)
    {
        BaseHoldItem heldItem = GiveItem();

        if (heldItem == null) return;

        SFX.Instance.PlayDropItem();
        var rb = heldItem.Rigidbody;
        rb.transform.position += (Vector3)(forceDirection * _dropOffset); // Moves the object collider away from the player collider
        rb.AddForce(forceDirection * _throwingForce, ForceMode2D.Impulse);
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
