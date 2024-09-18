using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateIdle : State<EnemyStates>
{
    IMove move;
    IIdle idle;
    Cooldown waitingTimeCooldown;

    public EnemyStateIdle(IIdle idle, IMove move)
    {
        this.move = move;
        this.idle = idle;
        waitingTimeCooldown = new(idle.WaitingTime, OnEndedIldeing);
    }

    private void OnEndedIldeing()
    {
        idle.IsIdleing = false;
    }

    public override void Enter()
    {
        base.Enter();
        move.Move(Vector3.zero);
        waitingTimeCooldown.ResetCooldown();
    }

    public override void Execute()
    {
        base.Execute();
        waitingTimeCooldown.IsCooldown();
    } 
}
