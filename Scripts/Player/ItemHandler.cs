using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    [SerializeField] private Transform _holdToolSpot;
    [SerializeField] private LayerMask _interectiveItemMask;
    [SerializeField] private float _distance = 1;
    [SerializeField] private GameObject _witchVisual;
    [SerializeField] private float _throwingForce;

    private PlayerController _playerController;
    private GameObject _itemHolding;
    private int _initialOrderInLayer;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }

    public void HandleItem()
    {
        if (_itemHolding)
        {
            _itemHolding.GetComponent<IHoldItem>().Use();
            return;
        }

        Vector2 direction = _playerController.FaceDirection.normalized;
        Vector2 origin = (Vector2)transform.position + direction * 0.1f;
        RaycastHit2D item = Physics2D.Raycast(origin, direction, _distance, _interectiveItemMask);

        if (item.rigidbody == null && item.collider == null) return;

        if (item.rigidbody != null && item.rigidbody.TryGetComponent(out IHoldItem holdItem))
            PickUp(item.rigidbody.gameObject);
        else if (item.collider.TryGetComponent(out ICookingStation cookingStation))
            cookingStation.Cook();
    }

    private void PickUp(GameObject holdItem)
    {
        _itemHolding = holdItem;

        var xScale = Mathf.Abs(_itemHolding.transform.localScale.x) *
            Mathf.Sign(_witchVisual.transform.localScale.x);
        _itemHolding.transform.localScale = new Vector2(xScale, _witchVisual.transform.localScale.y);

        _itemHolding.transform.position = _holdToolSpot.position;
        _itemHolding.transform.parent = _witchVisual.transform;

        _itemHolding.transform.rotation = Quaternion.identity;

        var spriteRender = _itemHolding.GetComponent<SpriteRenderer>();
        _initialOrderInLayer = spriteRender.sortingOrder;
        spriteRender.sortingOrder = _witchVisual.GetComponent<SpriteRenderer>().sortingOrder + 1;

        _itemHolding.GetComponent<Rigidbody2D>().simulated = false;
        _itemHolding.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }

    public void Drop()
    {
        if (!_itemHolding) return;
        var rb = _itemHolding.GetComponent<Rigidbody2D>();
        rb.AddForce(_playerController.FaceDirection.normalized * _throwingForce, ForceMode2D.Impulse);
        _itemHolding.transform.parent = null;
        _itemHolding.GetComponent<Rigidbody2D>().simulated = true;
        _itemHolding.GetComponent<SpriteRenderer>().sortingOrder = _initialOrderInLayer;
        _itemHolding = null;
    }

    private void OnDrawGizmos()
    {
        if (_playerController == null) return;
        Vector3 direction = _playerController.FaceDirection.normalized;
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, transform.position + direction * _distance);
    }
}
