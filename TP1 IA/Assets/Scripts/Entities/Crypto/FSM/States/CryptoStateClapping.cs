using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryptoStateClapping : State<CryptoStates>
{
    IMove move;
    IClapping clapping;

    Cooldown clappingDuration;

    public CryptoStateClapping(IMove move, IClapping clapping)
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
