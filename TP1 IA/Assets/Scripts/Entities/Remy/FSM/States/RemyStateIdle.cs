using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemyStateIdle : State<RemyStates>
{
    FSM<RemyStates> fsm;
    IMove move;
    IJump jump;
    IBreakDancing breakDancing;
    IDead dead;

    public RemyStateIdle(FSM<RemyStates> fsm, IMove move, IJump jump, IBreakDancing breakDancing, IDead dead)
    {
        this.fsm = fsm;
        this.move = move;
        this.jump = jump;
        this.breakDancing = breakDancing;
        this.dead = dead;
    }
    public override void Enter()
    {
        base.Enter();
        move.Move(Vector3.zero);
    }

    public override void FixedExecute()
    {
        base.FixedExecute();

        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");

        if (h != 0 || v != 0)
        {
            fsm.Transition(RemyStates.Move);
        }
        
        if (Input.GetKey(KeyCode.Space) && jump.CanJump())
        {
            fsm.Transition(RemyStates.Jump);
        }

        if (Input.GetKey(KeyCode.Return) && breakDancing.CanBreakDance())
        {
            fsm.Transition(RemyStates.BreakDancing);
        }

        if (dead.IsDead())
        {
            fsm.Transition(RemyStates.Dead);
        }
    }
}
