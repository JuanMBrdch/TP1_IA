using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Entity, IPatrol, IAttack, IIdle
{
    [Header("Patrolling")]
    [SerializeField] List<Transform> waypoints;    

    [Header("Attack")]
    public Entity target;
    [HideInInspector] public bool canSeePLayer;
    public float delayToLoosePlayer;
    public float delayTimer;
    [SerializeField] float attackRange;
    [SerializeField] float attackCooldown;
    [SerializeField] GameObject concreteAttack;
    [SerializeField] Transform attackSpawnPoint;

    [Header("Obstacle Avoidance")]
    public float radius;
    public float angle;
    public float personalArea;
    public LayerMask obsMask;
    ObstacleAvoidance _obs;
    public float timePrediction;

    bool isAttacking;

    protected override void Awake()
    {
        base.Awake();
        patrolFinished = true;
        _obs = new ObstacleAvoidance(transform, radius, angle, personalArea, obsMask);
    }
    
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }
    public List<Transform> Waypoints { get => waypoints; }
    public float AttackRange { get => attackRange; set => attackRange = value; }
    public float AttackCooldown { get => attackCooldown; set => attackCooldown = value; }
    public bool patrolFinished { get; set; }
    public bool IdleFinished { get; set; }    

    protected abstract void OnDestroy();
    protected void FinishedAttackActionHandler()
    {
        IsAttacking = false;
    }
    protected void ConcreteAttackActionHandler()
    {
        GameObject concreteAttackGO = Instantiate(concreteAttack, attackSpawnPoint.position, Quaternion.identity);
        concreteAttackGO.GetComponent<Attack>().Direction = transform.forward;
    }

    public override void Move(Vector3 dir)
    {
        Vector3 obsDir = _obs.GetDir(dir, false);
        obsDir.y = 0;
        dir.y = 0;
        base.Move(obsDir);
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
