using System.Linq;
using UnityEngine;

public class InteractionDetector : MonoBehaviour
{
    [SerializeField] private LayerMask _interectiveItemMask;
    [SerializeField] private float _interactionDistance = 1;
    [SerializeField] private float _radius = 0.1f;
    [SerializeField] private float _interactionOffset = 0.1f;

    private PlayerItemHolder _itemHolder;
    private PlayerController _playerController;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _itemHolder = GetComponent<PlayerItemHolder>();
    }

    public RaycastHit2D DetectInteractiveItem(bool hasHeldItem)
    {
        Vector2 direction = _playerController.InteractionDirection.normalized;
        Vector2 origin = (Vector2)transform.position + direction * _interactionOffset;

        RaycastHit2D[] hits = Physics2D.CircleCastAll(origin, _radius, direction,
            _interactionDistance, _interectiveItemMask);

        return SelectBestHit(hits);
    }

    private RaycastHit2D SelectBestHit(RaycastHit2D[] hits)
    {
        // priority on items that cannot be picked up if player has Item
        if (_itemHolder.HasItem())
            return hits
                .FirstOrDefault(item =>
                !item.collider.TryGetComponent<BaseHoldItem>(out _));

        // priority on items that can be picked up
        var heldItemHit = hits
            .FirstOrDefault(item =>
            item.collider.TryGetComponent<BaseHoldItem>(out _));

        if (heldItemHit != default) return heldItemHit;

        foreach (var hit in hits) 
        { 
            var item = hit.collider; 
            if (item.TryGetComponent(out IGiveHeldItem giver) && giver.HasItem()) 
                    return hit; 
        }

        return hits.FirstOrDefault();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_playerController == null) return;

        Vector2 direction = _playerController.InteractionDirection.normalized;
        Vector2 origin = (Vector2)transform.position + direction * _interactionOffset;

        Gizmos.color = Color.white;

        Gizmos.DrawWireSphere(origin, _radius);
        Vector2 end = origin + direction * _interactionDistance;
        Gizmos.DrawWireSphere(end, _radius);

        Gizmos.DrawLine(origin + Vector2.up * _radius, end + Vector2.up * _radius);
        Gizmos.DrawLine(origin - Vector2.up * _radius, end - Vector2.up * _radius);
        Gizmos.DrawLine(origin + Vector2.right * _radius, end + Vector2.right * _radius);
        Gizmos.DrawLine(origin - Vector2.right * _radius, end - Vector2.right * _radius);
    }
#endif
}
