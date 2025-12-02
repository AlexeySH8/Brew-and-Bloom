using UnityEngine;
public abstract class BaseHoldItem : MonoBehaviour
{
    [HideInInspector] public string PrefabPath;
    public Rigidbody2D Rigidbody { get; private set; }
    public ArcAnimation ArcAnimation { get; private set; }

    protected BaseItemHolder _currentHolder;
    protected SpriteRenderer _spriteRenderer;
    protected int _defaultSortingOrder;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _defaultSortingOrder = _spriteRenderer.sortingOrder;

        if (TryGetComponent(out ArcAnimation arc))
            ArcAnimation = arc;
        else
            ArcAnimation = gameObject.AddComponent<ArcAnimation>();

        ItemPool.Instance.Register(this);
    }

    public void SetHolder(BaseItemHolder newHolder)
    {
        _currentHolder?.OnItemRemoved(this);
        _currentHolder = newHolder;

        if (newHolder != null)
        {
            transform.SetParent(null);

            float holderFacing = Mathf.Sign(newHolder.ParentPoint.transform.localScale.x);
            Vector3 itemScale = transform.localScale;
            itemScale.x = Mathf.Abs(itemScale.x) * holderFacing;
            transform.localScale = itemScale;

            transform.SetParent(newHolder.ParentPoint);

            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            _spriteRenderer.sortingOrder = newHolder.SortingOrderOffset;

            Rigidbody.simulated = false;
            ItemPool.Instance.Unregister(this);
        }
        else
        {
            transform.SetParent(null);
            Rigidbody.simulated = true;
            _spriteRenderer.sortingOrder = _defaultSortingOrder;
            ItemPool.Instance.Register(this);
        }
    }

    public bool HasParent() => _currentHolder != null;

    public virtual void Discard()
    {
        SetHolder(null);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        ItemPool.Instance.Unregister(this);
    }
}
