using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Entity, IPatrol, IAttack, IIdle
{
    [Header("Patrolling")]
    [SerializeField] List<Transform> waypoints;
    [SerializeField] float waitingTime;

    [Header("Attack")]
    public Entity target;
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

    int currentPatrolWaypointID;
    int direction;

    protected override void Awake()
    {
        base.Awake();
        patrolFinished = true;
        _obs = new ObstacleAvoidance(transform, radius, angle, personalArea, obsMask);
        currentPatrolWaypointID = -1; // Todavia no tengo ningun waypoint alcanzado.
        direction = 1;
    }
    
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }
    public List<Transform> Waypoints { get => waypoints; }
    public float AttackRange { get => attackRange; set => attackRange = value; }
    public float AttackCooldown { get => attackCooldown; set => attackCooldown = value; }
    public bool patrolFinished { get; set; }
    public bool IdleFinished { get; set; }

    public int CurrentWaypointID => throw new System.NotImplementedException();

    public int Direction { get => direction; }
    public float WaitingTime { get => waitingTime; }

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
        base.Move(obsDir);
        Look(obsDir);
    }
    
    public virtual void Attack()
    {
        IsAttacking = true;
    }

    protected virtual void Start()
    {
        IsAttacking = false;
    }

    public void WaypointReached()
    {
        currentPatrolWaypointID += direction;

        // Cuando llego al final o al principio de la lista cambio la direccion.
        if (currentPatrolWaypointID == Waypoints.Count - 1 || currentPatrolWaypointID + direction == -1)
            direction *= -1;
    }
}
