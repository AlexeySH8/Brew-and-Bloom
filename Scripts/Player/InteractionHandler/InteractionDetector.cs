using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractionDetector : MonoBehaviour
{
    [SerializeField] private LayerMask _interectiveItemMask;
    [SerializeField] private float _interactionDistance = 1;
    [SerializeField] private float _radius = 0.1f;
    [SerializeField] private float _interactionOffset = 0.1f;
    private PlayerController _playerController;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }

    public RaycastHit2D DetectInterectiveItem(bool hasHeldItem)
    {
        Vector2 direction = _playerController.InteractionDirection.normalized;
        Vector2 origin = (Vector2)transform.position + direction * _interactionOffset;

        RaycastHit2D[] hits = Physics2D.CircleCastAll(origin, _radius, direction,
            _interactionDistance, _interectiveItemMask);

        // priority on items that cannot be picked up
        if (hasHeldItem)
            return hits
                .FirstOrDefault(item => !item.collider
                .TryGetComponent<BaseHoldItem>(out _));

        // priority on items that can be picked up
        var heldItem = hits
            .FirstOrDefault(item => item.collider
            .TryGetComponent<BaseHoldItem>(out _));
        return heldItem != default ? heldItem : hits.FirstOrDefault();
    }

    public Collider2D DetectToolTarget(float interactionToolDistance, LayerMask interactionMask)
    {
        Vector2 direction = new Vector2(_playerController.FaceDirection, 0);
        Vector2 origin = transform.position;
        RaycastHit2D toolTarget = Physics2D.Raycast(origin, direction,
            interactionToolDistance, interactionMask);
        Debug.DrawRay(origin, direction * interactionToolDistance, Color.red, 0.5f);
        return toolTarget.collider;
    }

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
}
