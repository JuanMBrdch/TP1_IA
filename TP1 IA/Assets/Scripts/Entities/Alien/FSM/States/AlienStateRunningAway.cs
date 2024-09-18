using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienStateRunningAway : State<AlienStates>
{
    IMove move;
    IRunAway runningAway;
    Evade evade;

    Cooldown runningAwayCooldownDuration;

    public AlienStateRunningAway(IMove move, IRunAway runningAway, Transform entity, Rigidbody target, float timePrediction)
    {
        this.move = move;
        this.runningAway = runningAway;
        runningAwayCooldownDuration = new Cooldown(runningAway.RunAwayDuration, OnEndedRunningAway);
        evade = new(entity, target, timePrediction);
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

        Vector3 moveDir = evade.GetDir();
        move.Move(moveDir);
        move.Look(moveDir);

        runningAwayCooldownDuration.IsCooldown();
    }
}
