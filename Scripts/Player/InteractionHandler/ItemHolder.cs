using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    [SerializeField] private Transform _holdToolSpot;
    [SerializeField] private Transform _holdIngredientSpot;
    [SerializeField] private GameObject _witchVisual;
    [SerializeField] private float _throwingForce;

    private GameObject _heldItem;
    private int _initialOrderInLayer;

    public void PickUp(GameObject holdItem)
    {
        _heldItem = holdItem;

        var xScale = Mathf.Abs(_heldItem.transform.localScale.x) *
            Mathf.Sign(_witchVisual.transform.localScale.x);
        _heldItem.transform.localScale = new Vector2(xScale, _witchVisual.transform.localScale.y);

        if (holdItem.TryGetComponent(out BaseTool tool))
            _heldItem.transform.position = _holdToolSpot.position;
        else if (holdItem.TryGetComponent(out BaseIngredient ingredient))
            _heldItem.transform.position = _holdIngredientSpot.position;
        else
            Debug.LogWarning("Unknown heir of IHoldItem");

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
        rb.simulated = true;
        rb.AddForce(forceVector * _throwingForce, ForceMode2D.Impulse);
        _heldItem.transform.parent = null;
        _heldItem.GetComponent<SpriteRenderer>().sortingOrder = _initialOrderInLayer;
        Clear();
    }

    public GameObject GetHeldItem() => _heldItem;

    public void Clear()
    {
        _heldItem = null;
    }
}
