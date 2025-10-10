using UnityEngine;

public abstract class BaseHoldItem : MonoBehaviour
{
    public Rigidbody2D Rigidbody { get; private set; }

    protected IItemHolder _currentHolder;
    protected SpriteRenderer _spriteRenderer;
    protected int _defaultSortingOrder;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _defaultSortingOrder = _spriteRenderer.sortingOrder;
    }

    public void SetHolder(IItemHolder newHolder)
    {
        _currentHolder?.ItemRemoved(this);
        _currentHolder = newHolder;

        if (newHolder != null)
        {
            transform.SetParent(newHolder.ParentPoint, true);

            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            _spriteRenderer.sortingOrder = newHolder.SortingOrderOffset;

            Rigidbody.simulated = false;

            newHolder.ItemReceived(this);
        }
        else
        {
            transform.SetParent(null);
            Rigidbody.simulated = true;
            _spriteRenderer.sortingOrder = _defaultSortingOrder;
        }
    }

    public virtual void Use(Collider2D target) { }

    public virtual void Discard()
    {
        SetHolder(null);
        Destroy(gameObject);
    }
}
