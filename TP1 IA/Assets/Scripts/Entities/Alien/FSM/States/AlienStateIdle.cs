using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienStateIdle : State<AlienStates>
{
    IMove move;
    public AlienStateIdle(IMove move)
    {
        this.move = move;
    }

    public override void Enter()
    {
        base.Enter();
        move.Move(Vector3.zero);
    }
}
