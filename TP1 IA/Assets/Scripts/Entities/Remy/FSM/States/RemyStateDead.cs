using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemyStateDead : State<RemyStates>
{
    FSM<RemyStates> fsm;
    IMove move;
    IDead dead;

    public RemyStateDead(FSM<RemyStates> fsm, IMove move, IDead dead)
    {
        this.fsm = fsm;
        this.move = move;
        this.dead = dead;
    }
    public override void Enter()
    {
        base.Enter();
        dead.Die();
        move.Move(Vector3.zero);
    }

    public override void Execute()
    {
        base.Execute();
    }
}
