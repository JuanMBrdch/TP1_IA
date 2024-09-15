using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemyStateBreakDancing : State<RemyStates>
{
    FSM<RemyStates> fsm;
    IMove move;
    IJump jump;
    IBreakDancing breakDancing;

    public RemyStateBreakDancing(FSM<RemyStates> fsm, IMove move, IJump jump, IBreakDancing breakDancing)
    {
        this.fsm = fsm;
        this.move = move;
        this.jump = jump;
        this.breakDancing = breakDancing;
    }

    public override void Enter()
    {
        base.Enter();
        breakDancing.BreakDance();
        move.Move(Vector3.zero);
    }

    public override void Execute()
    {
        base.Execute();

        breakDancing.UpdateCooldowns();
        if (breakDancing.IsBreakDancing)
            return;

        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");

        if (h == 0 && v == 0)
        {
            fsm.Transition(RemyStates.Idle);
        }

        else
        {
            fsm.Transition(RemyStates.Move);
        }
    }
}
