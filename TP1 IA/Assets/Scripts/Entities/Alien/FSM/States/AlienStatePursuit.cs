using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienStatePursuit : State<AlienStates>
{
    IMove move;
    Transform entity;
    Transform target;
    public AlienStatePursuit(IMove move, Transform entity, Transform target)
    {
        this.move = move;
        this.entity = entity;
        this.target = target;
    }

    public override void Execute()
    {
        // TODO Implement Pursuit With Obstacle Avoidance
        base.Execute();

        Vector3 dirToTarget = target.position - entity.position;
        move.Move(dirToTarget.normalized);
        dirToTarget.y = 0;
        move.Look(dirToTarget);
    }
}
