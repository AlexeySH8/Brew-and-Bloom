using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractionDetector : MonoBehaviour
{
    [SerializeField] private LayerMask _interectiveItemMask;
    [SerializeField] private float _distance = 1;
    [SerializeField] private float _radius = 0.1f;
    private PlayerController _playerController;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }

    public RaycastHit2D DetectInterectiveItem()
    {
        Vector2 direction = _playerController.InteractionDirection.normalized;
        Vector2 origin = (Vector2)transform.position + direction * 0.1f;
        RaycastHit2D interactiveItem = Physics2D.CircleCast(origin, _radius, direction,
            _distance, _interectiveItemMask);
        return interactiveItem;
    }

    public Collider2D DetectToolTrget(float interactionDistance, LayerMask interactionMask)
    {
        Vector2 direction = new Vector2(_playerController.FaceDirection, 0);
        Vector2 origin = transform.position;
        RaycastHit2D toolTarget = Physics2D.Raycast(origin, direction,
            interactionDistance, interactionMask);
        Debug.DrawRay(origin, direction * interactionDistance, Color.red, 0.5f);
        return toolTarget.collider;
    }

    private void OnDrawGizmos()
    {
        if (_playerController == null) return;

        Vector2 direction = _playerController.InteractionDirection.normalized;
        Vector2 origin = (Vector2)transform.position + direction * 0.1f;

        Gizmos.color = Color.white;

        Gizmos.DrawWireSphere(origin, _radius);
        Vector2 end = origin + direction * _distance;
        Gizmos.DrawWireSphere(end, _radius);

        Gizmos.DrawLine(origin + Vector2.up * _radius, end + Vector2.up * _radius);
        Gizmos.DrawLine(origin - Vector2.up * _radius, end - Vector2.up * _radius);
        Gizmos.DrawLine(origin + Vector2.right * _radius, end + Vector2.right * _radius);
        Gizmos.DrawLine(origin - Vector2.right * _radius, end - Vector2.right * _radius);
    }
}
