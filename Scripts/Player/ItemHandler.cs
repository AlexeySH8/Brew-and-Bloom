using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    [SerializeField] private Transform _holdToolSpot;
    [SerializeField] private LayerMask _interectiveItemMask;
    [SerializeField] private float _distance = 1;
    [SerializeField] private GameObject _witchVisual;
    [SerializeField] private float _throwingForce;

    private PlayerController _playerController;
    private GameObject _holdingItem;
    private int _initialOrderInLayer;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }

    public void HandleItem()
    {
        Vector2 direction = _playerController.FaceDirection.normalized;
        Vector2 origin = (Vector2)transform.position + direction * 0.1f;
        RaycastHit2D objInFront = Physics2D.Raycast(origin, direction, _distance, _interectiveItemMask);

        if (_holdingItem)
        {
            if (objInFront.collider != null && objInFront.collider.TryGetComponent(out ICookingStation cookingStation))
            {
                cookingStation.Cook(_holdingItem);
                _holdingItem = null;
                return;
            }
            else if (_holdingItem.TryGetComponent(out BaseTool tool))
            {
                tool.Use();
                return;
            }
        }
        else if (objInFront.rigidbody != null && objInFront.rigidbody.TryGetComponent(out IHoldItem holdItem))
        {
            PickUp(objInFront.rigidbody.gameObject);
            return;
        }
    }

    private void PickUp(GameObject holdItem)
    {
        _holdingItem = holdItem;

        var xScale = Mathf.Abs(_holdingItem.transform.localScale.x) *
            Mathf.Sign(_witchVisual.transform.localScale.x);
        _holdingItem.transform.localScale = new Vector2(xScale, _witchVisual.transform.localScale.y);

        _holdingItem.transform.position = _holdToolSpot.position;
        _holdingItem.transform.parent = _witchVisual.transform;

        _holdingItem.transform.rotation = Quaternion.identity;

        var spriteRender = _holdingItem.GetComponent<SpriteRenderer>();
        _initialOrderInLayer = spriteRender.sortingOrder;
        spriteRender.sortingOrder = _witchVisual.GetComponent<SpriteRenderer>().sortingOrder + 1;

        _holdingItem.GetComponent<Rigidbody2D>().simulated = false;
        _holdingItem.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }

    public void Drop()
    {
        if (!_holdingItem) return;
        var rb = _holdingItem.GetComponent<Rigidbody2D>();
        rb.simulated = true;
        rb.AddForce(_playerController.FaceDirection.normalized * _throwingForce, ForceMode2D.Impulse);
        _holdingItem.transform.parent = null;
        _holdingItem.GetComponent<SpriteRenderer>().sortingOrder = _initialOrderInLayer;
        _holdingItem = null;
    }

    private void OnDrawGizmos()
    {
        if (_playerController == null) return;
        Vector3 direction = _playerController.FaceDirection.normalized;
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, transform.position + direction * _distance);
    }
}
