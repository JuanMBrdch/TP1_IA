using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryptoStatePursuit : State<CryptoStates>
{
    IMove move;
    Transform entity;
    Transform target;
    public CryptoStatePursuit(IMove move, Transform entity, Transform target)
    {
        this.move = move;
        this.entity = entity;
        this.target = target;
    }

    public override void Execute()
    {
        base.Execute();

        Vector3 dirToTarget = target.position - entity.position;
        move.Move(dirToTarget.normalized);
        dirToTarget.y = 0;
        move.Look(dirToTarget);
    }
}
