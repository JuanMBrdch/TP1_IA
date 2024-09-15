using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemyStateJump : State<RemyStates>
{
    private FSM<RemyStates> fsm;
    private IMove move;
    private IJump jump;

    public RemyStateJump(FSM<RemyStates> fsm, IMove move, IJump jump)
    {
        this.fsm = fsm;
        this.move = move;
        this.jump = jump;
    }

    public override void Enter()
    {
        base.Enter();
        jump.Jump();
    }
    public override void FixedExecute()
    {
        base.FixedExecute();

        if (jump.IsJumping)
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
