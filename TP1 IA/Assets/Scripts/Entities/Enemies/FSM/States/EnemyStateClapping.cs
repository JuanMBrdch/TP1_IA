using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateClapping : State<EnemyStates>
{
    IMove move;
    IClapping clapping;

    Cooldown clappingDuration;

    public EnemyStateClapping(IMove move, IClapping clapping)
    {
        this.move = move;
        this.clapping = clapping;
        clappingDuration = new Cooldown(clapping.ClappingDuration, OnEndedClapping);
    }
    private void OnEndedClapping()
    {
        clapping.IsClapping = false;
    }

    public override void Enter()
    {
        base.Enter();
        clapping.Clap();
        move.Move(Vector3.zero);
        clappingDuration.ResetCooldown();
    }

    public override void Execute()
    {
        base.Execute();
        clappingDuration.IsCooldown();
    }
}
