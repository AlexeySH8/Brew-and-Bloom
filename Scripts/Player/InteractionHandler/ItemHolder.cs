using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    [SerializeField] private Transform _holdToolSpot;
    [SerializeField] private Transform _holdItemSpot;
    [SerializeField] private GameObject _witchVisual;
    [SerializeField] private float _throwingForce;

    private GameObject _heldItem;
    private int _initialOrderInLayer;

    public GameObject GetHeldItem() => _heldItem;

    public void PickUp(GameObject holdItem)
    {
        _heldItem = holdItem;

        var xScale = Mathf.Abs(_heldItem.transform.localScale.x) *
            Mathf.Sign(_witchVisual.transform.localScale.x);
        _heldItem.transform.localScale = new Vector2(xScale, _witchVisual.transform.localScale.y);

        if (holdItem.TryGetComponent(out BaseTool tool))
            _heldItem.transform.position = _holdToolSpot.position;
        else
            _heldItem.transform.position = _holdItemSpot.position;

        _heldItem.transform.parent = _witchVisual.transform;
        _heldItem.transform.rotation = Quaternion.identity;

        var spriteRender = _heldItem.GetComponent<SpriteRenderer>();
        _initialOrderInLayer = spriteRender.sortingOrder;
        spriteRender.sortingOrder = _witchVisual.GetComponent<SpriteRenderer>().sortingOrder + 1;

        _heldItem.GetComponent<Rigidbody2D>().simulated = false;
        _heldItem.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }

    public void Drop(Vector2 forceVector)
    {
        if (!_heldItem) return;
        var rb = _heldItem.GetComponent<Rigidbody2D>();        
        rb.transform.position += (Vector3)(forceVector * 0.2f); // Moves the object collider away from the player collider
        Clear();
        rb.AddForce(forceVector * _throwingForce, ForceMode2D.Impulse);
    }

    public void Clear()
    {
        _heldItem.GetComponent<Rigidbody2D>().simulated = true;
        _heldItem.GetComponent<SpriteRenderer>().sortingOrder = _initialOrderInLayer;
        _heldItem.transform.parent = null;
        _heldItem = null;
    }
}
