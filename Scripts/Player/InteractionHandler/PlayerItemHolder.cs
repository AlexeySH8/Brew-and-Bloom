using UnityEngine;

public class PlayerItemHolder : MonoBehaviour, IItemHolder
{
    [SerializeField] private Transform _holdToolPoint;
    [SerializeField] private Transform _holdItemPoint;
    [SerializeField] private float _throwingForce;

    private PlayerVisual _witchVisual;
    private BaseHoldItem _heldItem;
    private Rigidbody2D _heldItemRB;


    public Transform ParentPoint => _witchVisual.transform;
    public int SortingOrderOffset => _witchVisual.SpriteRenderer.sortingOrder + 1;

    private void Awake()
    {
        _witchVisual = GetComponentInChildren<PlayerVisual>();
    }

    public void PickUp(BaseHoldItem holdItem)
    {
        holdItem.SetHolder(this);
        _heldItem = holdItem;
        SetItemPosition();
    }

    public void Drop(Vector2 forceVector)
    {
        if (!_heldItem) return;

        var rb = _heldItem.Rigidbody;
        rb.transform.position += (Vector3)(forceVector * 0.2f); // Moves the object collider away from the player collider
        _heldItem.SetHolder(null);
        rb.AddForce(forceVector * _throwingForce, ForceMode2D.Impulse);
    }

    public void OnItemRemoved(BaseHoldItem holdItem)
    {
        if (_heldItem == null || _heldItem != holdItem) return;
        _heldItem = null;
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
