using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateIdle<T> : State<T>
{
    IMove move;

    public PlayerStateIdle(IMove move)
    {
        this.move = move;
    }

    public override void Enter()
    {
        base.Enter();
        move.Move(Vector3.zero);
    }
}
