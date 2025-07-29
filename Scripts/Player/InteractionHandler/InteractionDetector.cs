using System.Collections;
using System.Collections.Generic;
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

    public RaycastHit2D Detect()
    {
        Vector2 direction = _playerController.InteractionDirection.normalized;
        Vector2 origin = (Vector2)transform.position + direction * 0.1f;
        RaycastHit2D objInFront = Physics2D.CircleCast(origin, _radius, direction, _distance, _interectiveItemMask);

        return objInFront;
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
