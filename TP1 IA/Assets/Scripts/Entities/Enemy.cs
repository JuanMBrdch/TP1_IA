using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Entity, IPatrol, IAttack
{
    [Header("References")]
    [SerializeField] List<Transform> waypoints;
    [SerializeField] float attackRange;
    [SerializeField] float attackCooldown;

    bool isAttacking;

    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }
    public List<Transform> Waypoints { get => waypoints; }
    public float AttackRange { get => attackRange; set => attackRange = value; }
    public float AttackCooldown { get => attackCooldown; set => attackCooldown = value; }

    protected abstract void OnDestroy();
    protected void FinishedAttackActionHandler()
    {
        IsAttacking = false;
    }
    protected abstract void ConcreteAttackActionHandler();// Momento exacto en el que el ataque puede hacer daño

    public virtual void Attack()
    {
        IsAttacking = true;
    }
}
