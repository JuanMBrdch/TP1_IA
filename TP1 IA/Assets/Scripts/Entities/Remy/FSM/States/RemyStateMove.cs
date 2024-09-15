using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RemyStateMove : State<RemyStates>
{
    FSM<RemyStates> fsm;
    IMove move;
    IJump jump;
    IBreakDancing breakDancing;
    IDead dead;

    public RemyStateMove(FSM<RemyStates> fsm, IMove move, IJump jump, IBreakDancing breakDancing, IDead dead)
    {
        this.fsm = fsm;
        this.move = move;
        this.jump = jump;
        this.breakDancing = breakDancing;
        this.dead = dead;
    }

    public override void FixedExecute()
    {
        base.FixedExecute();

        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");

        bool isInputACtive = Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0;

        if (h == 0 && v == 0)
        {
            fsm.Transition(RemyStates.Idle);
        }
        else
        {
            // Forma cabezona para que tenga el smooth del GetAxis 
            // pero que al moverse en diagonal no se mueva mas rapido.

            if (h != 0 && v != 0)
            {
                h *= 0.70710678f;
                v *= 0.70710678f;
            }

            Vector3 dir = new Vector3(h, 0, v);

            move.Move(dir);

            if (isInputACtive)
                move.Look(dir);
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
