using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class RemyController : MonoBehaviour
{
    IMove _move;
    IJump _jump;
    IBreakDancing _breakDancing;
    IDead _dead;

    FSM<RemyStates> fsm;
    void Start()
    {
        InitFSM();
    }
    void InitFSM()
    {
        _move = GetComponent<IMove>();
        _jump = GetComponent<IJump>();
        _breakDancing = GetComponent<IBreakDancing>();
        _dead = GetComponent<IDead>();

        fsm = new();

        var idle = new RemyStateIdle(fsm, _move, _jump, _breakDancing, _dead);
        var move = new RemyStateMove(fsm, _move, _jump, _breakDancing, _dead);
        var jump = new RemyStateJump(fsm, _move, _jump);
        var breakDancing = new RemyStateBreakDancing(fsm, _move, _jump, _breakDancing);
        var dead = new RemyStateDead(fsm, _move, _dead);

        idle.AddTransition(RemyStates.Move, move);
        idle.AddTransition(RemyStates.Jump, jump);
        idle.AddTransition(RemyStates.BreakDancing, breakDancing);
        idle.AddTransition(RemyStates.Dead, dead);

        move.AddTransition(RemyStates.Idle, idle);
        move.AddTransition(RemyStates.Jump, jump);
        move.AddTransition(RemyStates.BreakDancing, breakDancing);
        move.AddTransition(RemyStates.Dead, dead);

        jump.AddTransition(RemyStates.Idle, idle);
        jump.AddTransition(RemyStates.Move, move);

        breakDancing.AddTransition(RemyStates.Idle, idle);
        breakDancing.AddTransition(RemyStates.Move, move);

        fsm.SetInitial(idle);
    }
    void Update()
    {
        fsm.OnUpdate();
    }
    private void FixedUpdate()
    {
        fsm.OnFixedUpdate();
    }
    private void LateUpdate()
    {
        fsm.OnLateUpdate();
    }
}
