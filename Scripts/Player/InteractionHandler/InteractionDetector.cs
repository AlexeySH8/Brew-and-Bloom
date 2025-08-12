using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractionDetector : MonoBehaviour
{
    [SerializeField] private LayerMask _interectiveItemMask;
    [SerializeField] private float _interactionDistance = 1;
    [SerializeField] private float _radius = 0.1f;
    private PlayerController _playerController;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }

    public RaycastHit2D DetectInterectiveItem(bool hasHeldItem)
    {
        Vector2 direction = _playerController.InteractionDirection.normalized;
        Vector2 origin = (Vector2)transform.position + direction * 0.1f;
        RaycastHit2D[] interactiveItems = Physics2D.CircleCastAll(origin, _radius, direction,
            _interactionDistance, _interectiveItemMask);

        if (!hasHeldItem) 
        {
            var heldItem = interactiveItems
                .FirstOrDefault(item => item.collider.TryGetComponent<IHoldItem>(out _));
            if(heldItem) return heldItem;
        }
        return interactiveItems.FirstOrDefault();
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
        Vector2 origin = (Vector2)transform.position + direction * 0.1f;

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
