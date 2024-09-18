using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryptoStatePursuit : State<CryptoStates>
{
    IMove move;
    Transform entity;
    Rigidbody target; 
    float timePrediction;

    public CryptoStatePursuit(IMove move, Transform entity, Rigidbody target, float timePrediction)
    {
        this.move = move;
        this.entity = entity;
        this.target = target;
        this.timePrediction = timePrediction;
    }

    public override void Execute()
    {
        base.Execute();
        Vector3 futurePosition = target.position + target.velocity * timePrediction;
        Vector3 dirToFuturePosition = (futurePosition - entity.position).normalized;
        Vector3 dirToTarget = (target.position - entity.position).normalized;
        
        if (Vector3.Dot(dirToFuturePosition, dirToTarget) < 0)
        {
            move.Move(dirToTarget); 
        }
        else
        {
            move.Move(dirToFuturePosition); 
        }
        dirToFuturePosition.y = 0;
        move.Look(dirToFuturePosition);
    }
}
