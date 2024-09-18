using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateIdle : State<EnemyStates>
{
    IMove move;
    public EnemyStateIdle(IMove move)
    {
        this.move = move;
    }

    public override void Enter()
    {
        base.Enter();
        move.Move(Vector3.zero);
    }
}
