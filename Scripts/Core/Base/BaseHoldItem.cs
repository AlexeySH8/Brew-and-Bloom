using UnityEngine;
using Zenject;

public abstract class BaseHoldItem : MonoBehaviour
{
    public Rigidbody2D Rigidbody { get; private set; }

    protected BaseItemHolder _currentHolder;
    protected SpriteRenderer _spriteRenderer;
    protected int _defaultSortingOrder;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _defaultSortingOrder = _spriteRenderer.sortingOrder;

        ItemPool.Instance.Register(this);
        CheckParent();
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
        }
        else
        {
            transform.SetParent(null);
            Rigidbody.simulated = true;
            _spriteRenderer.sortingOrder = _defaultSortingOrder;
        }
    }

    private void CheckParent()
    {
        Transform parent = transform.parent;
        if (parent != null && !parent.TryGetComponent<BaseItemHolder>(out _))
            Debug.LogError($"The parent of {gameObject.name} does not implement the IItemHolder");
    }

    public virtual void Discard()
    {
        ItemPool.Instance.Unregister(this);
        SetHolder(null);
        Destroy(gameObject);
    }
}
