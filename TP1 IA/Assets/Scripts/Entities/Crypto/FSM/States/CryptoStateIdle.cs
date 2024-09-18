using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryptoStateIdle : State<CryptoStates>
{
    IMove move;
    public CryptoStateIdle(IMove move)
    {
        this.move = move;
    }

    public override void Enter()
    {
        base.Enter();
        move.Move(Vector3.zero);
    }
}
