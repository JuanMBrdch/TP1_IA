using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateIdle : State<EnemyStates>
{
    IMove move;
    IIdle idle;

    Cooldown waitingTime;

    public EnemyStateIdle(IIdle idle, IMove move)
    {
        this.move = move;
        this.idle = idle;
        waitingTime = new(idle.WaitingTime);
    }

    public override void Enter()
    {
        base.Enter();
        move.Move(Vector3.zero);
        waitingTime.ResetCooldown();
    }

    public override void Execute()
    {
        base.Execute();
        waitingTime.IsCooldown();
    } 
}
