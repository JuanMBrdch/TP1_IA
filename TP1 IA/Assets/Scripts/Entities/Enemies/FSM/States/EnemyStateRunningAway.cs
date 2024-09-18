using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateRunningAway : State<EnemyStates>
{
    IMove move;
    IRunAway runningAway;

    Cooldown runningAwayCooldownDuration;

    public EnemyStateRunningAway(IMove move, IRunAway runningAway)
    {
        this.move = move;
        this.runningAway = runningAway;
        runningAwayCooldownDuration = new Cooldown(runningAway.RunAwayDuration, OnEndedRunningAway);
    }
    private void OnEndedRunningAway()
    {
        runningAway.IsRunningAway = false;
    }

    public override void Enter()
    {
        base.Enter();
        runningAway.RunAway();
        runningAwayCooldownDuration.ResetCooldown();
    }

    public override void Execute()
    {
        base.Execute();
        runningAwayCooldownDuration.IsCooldown();
    }
}
