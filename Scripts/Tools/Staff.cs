using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : BaseUsableItem
{
    [SerializeField] private float _radius;

    public override bool TryUse()
    {
        Collider2D target = DetectTarget();
        if (target != null &&
            target.TryGetComponent(out IStaffTarget staffTarget))
        {
            staffTarget.InteractWithStaff();
            return true;
        }
        return false;
    }

    protected override Collider2D DetectTarget()
    {
        float itemFacing = Mathf.Sign(_currentHolder.ParentPoint.localScale.x);
        Vector2 direction = new Vector2(itemFacing, 0);
        Vector2 origin = transform.position;

        RaycastHit2D toolTarget = Physics2D.CircleCast(origin, _radius, direction,
            InteractionDistance, InteractionMask);

        return toolTarget.collider;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_currentHolder == null)
            return;

        float itemFacing = Mathf.Sign(_currentHolder.ParentPoint.localScale.x);
        Vector2 direction = new Vector2(itemFacing, 0);
        Vector2 origin = transform.position;
        Vector2 endPoint = origin + direction * InteractionDistance;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(origin, _radius);
        Gizmos.DrawWireSphere(endPoint, _radius);
        Gizmos.DrawLine(origin, endPoint);
    }
#endif
}
