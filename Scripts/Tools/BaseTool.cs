using UnityEngine;

public abstract class BaseTool : MonoBehaviour, IHoldItem
{
    public LayerMask InteractionMask { get; private set; }
    public float InteractionDistance { get; private set; }
    [SerializeField] protected LayerMask _interactionMask;
    [SerializeField] protected float _interactionDistance = 0.5f;

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
        InteractionDistance = _interactionDistance;
        InteractionMask = _interactionMask;
    }

    public abstract void Use(Collider2D target);

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
