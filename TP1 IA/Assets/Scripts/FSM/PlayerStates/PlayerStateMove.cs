using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMove<T> : State<T>
{
    IMove move;

    public PlayerStateMove(IMove move)
    {
        this.move = move;
    }
    public override void Execute()
    {
        base.Execute();
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v);

        move.Move(dir.normalized);

        if (h != 0 || v != 0)
        {
            move.Look(dir.normalized);
        }

    }
}
