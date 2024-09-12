using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Entity
{
    [Header("References")]
    [SerializeField] protected Transform target;

    [Header("Properties")]
    [SerializeField] protected float pursuitRadius;

    public Transform Target { get => target; set => target = value; }

    protected bool IsTargetCloseEnough { get { return (target.position - transform.position).magnitude <= pursuitRadius; } }

    bool isAttacking;
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }

    protected abstract void Awake();
    protected abstract void OnDestroy();
    protected void FinishedAttackActionHandler()
    {
        IsAttacking = false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, pursuitRadius);
    }
#endif
}
