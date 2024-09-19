using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatePursuit : State<EnemyStates>
{
    IMove move;
    Pursuit pursuit;

    public EnemyStatePursuit(IMove move, Transform entity, Rigidbody target, float timePrediction)
    {
        this.move = move;
        pursuit = new(entity, target, timePrediction);
    }

    public override void FixedExecute()
    {
        base.FixedExecute();

        Vector3 pursuitDir = pursuit.GetDir();
        move.Move(pursuitDir);
    }
}
