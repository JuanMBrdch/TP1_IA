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

    [Header("Obstacle Avoidance")]
    public float radius;
    public float angle;
    public float personalArea;
    public LayerMask obsMask;
    ObstacleAvoidance _obs;
    protected override void Awake()
    {
        base.Awake();
        _obs = new ObstacleAvoidance(transform, radius, angle, personalArea, obsMask);
    }
    
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }
    public List<Transform> Waypoints { get => waypoints; }
    public float AttackRange { get => attackRange; set => attackRange = value; }
    public float AttackCooldown { get => attackCooldown; set => attackCooldown = value; }

    protected abstract void OnDestroy();
    protected void FinishedAttackActionHandler()
    {
        IsAttacking = false;
    }
    protected abstract void ConcreteAttackActionHandler();// Momento exacto en el que el ataque puede hacer da�o

    public override void Move(Vector3 dir)
    {
        dir = _obs.GetDir(dir, false);
        dir.y = 0;
        Look(dir);
        base.Move(dir);
    }
    
    public virtual void Attack()
    {
        IsAttacking = true;
    }

    protected override void Start()
    {
        base.Start();
        IsAttacking = false;
    }
}
