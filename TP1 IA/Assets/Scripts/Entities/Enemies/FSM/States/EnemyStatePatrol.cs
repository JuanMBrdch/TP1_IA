using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatePatrol : State<EnemyStates>
{
    Transform entity;
    List<Transform> waypoints;

    IIdle idle;
    IMove move;
    IPatrol patrol;
    public EnemyStatePatrol(IIdle idle, IMove move, Transform entity, IPatrol patrol)
    {
        this.idle = idle;
        this.move = move;
        this.entity = entity;
        this.patrol = patrol;
        this.waypoints = patrol.Waypoints;
    }

    public override void Execute()
    {
        base.Execute();

        Vector3 nextWaypoint = waypoints[patrol.CurrentWaypointID + patrol.Direction].position;
        Vector3 nextWaypointDirection = (nextWaypoint - entity.position).normalized;

        move.Move(nextWaypointDirection);

        if (Vector3.Distance(entity.position, nextWaypoint) <= 0.1f)
        {
            patrol.WaypointReached();
            idle.IsIdleing = true;
        }
    }
}
