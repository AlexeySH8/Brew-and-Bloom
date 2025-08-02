using UnityEngine;

public abstract class BaseTool : MonoBehaviour, IHoldItem
{
    [SerializeField] protected float _interactionDistance = 0.5f;
    [SerializeField] protected LayerMask _interectiveItemMask;

    private GameObject _shelf;
    protected Vector2 _positionOnShelf;
    protected Vector2 _initialScale;
    private int _initialOrderInLayer;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;

    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _shelf = GameObject.FindWithTag("Shelf");
        _positionOnShelf = transform.position;
        _initialScale = transform.localScale;
        _initialOrderInLayer = _spriteRenderer.sortingOrder;
    }

    public abstract void Use(float faceDirection);

    protected virtual Collider2D Detect(float faceDirection)
    {
        Vector2 origin = transform.position;
        Vector2 direction = new Vector2(faceDirection, 0);
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, _interactionDistance, _interectiveItemMask);
        Debug.DrawRay(origin, direction * _interactionDistance, Color.red, 0.5f);
        return hit.collider;
    }

    public virtual void Discard()
    {
        transform.parent = _shelf.transform;
        transform.position = _positionOnShelf;
        transform.rotation = Quaternion.identity;
        transform.localScale = _initialScale;
        _spriteRenderer.sortingOrder = _initialOrderInLayer;
        _rb.simulated = true;
        _rb.bodyType = RigidbodyType2D.Kinematic;
        _rb.velocity = Vector2.zero;
        _rb.angularVelocity = 0f;
    }
}
