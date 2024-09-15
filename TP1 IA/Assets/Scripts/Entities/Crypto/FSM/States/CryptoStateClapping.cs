using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryptoStateClapping : State<CryptoStates>
{
    IMove move;
    IClapping clapping;
    public CryptoStateClapping(IMove move, IClapping clapping)
    {
        this.move = move;
        this.clapping = clapping;
    }
    public override void Enter()
    {
        base.Enter();
        // clapping.Clap();
        // move.Move(Vector3.zero);
    }

    public override void Execute()
    {
        base.Execute();
        clapping.Clap();
        move.Move(Vector3.zero);
    }
}
