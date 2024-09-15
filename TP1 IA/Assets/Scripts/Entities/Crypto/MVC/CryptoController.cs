using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CryptoController : MonoBehaviour
{
    public Transform target;

    FSM<CryptoStates> fsm;

    IMove _move;
    IPatrol _patrol;
    IClapping _clapping;
    IAttack _attack;

    void Start()
    {
        InitFSM();
    }
    void InitFSM()
    {
        _move = GetComponent<IMove>();
        _patrol = GetComponent<IPatrol>();
        _clapping = GetComponent<IClapping>();
        _attack = GetComponent<IAttack>();

        fsm = new();

        var idle = new CryptoStateIdle(_move);
        var patrol = new CryptoStatePatrol(_move, this.transform, _patrol);
        var pursuit = new CryptoStatePursuit(_move, this.transform, target);
        var clap = new CryptoStateClapping(_move, _clapping);   
        var attack = new CryptoStateAttacking(_move, _attack, target);

        idle.AddTransition(CryptoStates.Patrol, patrol);
        idle.AddTransition(CryptoStates.Pursuit, pursuit);
        idle.AddTransition(CryptoStates.Clap, clap);
        idle.AddTransition(CryptoStates.Attack, attack);

        patrol.AddTransition(CryptoStates.Idle, idle);
        patrol.AddTransition(CryptoStates.Pursuit, pursuit);
        patrol.AddTransition(CryptoStates.Clap, clap);
        patrol.AddTransition(CryptoStates.Attack, attack);

        pursuit.AddTransition(CryptoStates.Idle, idle);
        pursuit.AddTransition(CryptoStates.Patrol, patrol);
        pursuit.AddTransition(CryptoStates.Clap, clap);
        pursuit.AddTransition(CryptoStates.Attack, attack);

        clap.AddTransition(CryptoStates.Idle, idle);
        clap.AddTransition(CryptoStates.Patrol, patrol);
        clap.AddTransition(CryptoStates.Pursuit, pursuit);
        clap.AddTransition(CryptoStates.Attack, attack);

        attack.AddTransition(CryptoStates.Idle, idle);
        attack.AddTransition(CryptoStates.Patrol, patrol);
        attack.AddTransition(CryptoStates.Pursuit, pursuit);
        attack.AddTransition(CryptoStates.Clap, clap);

        fsm.SetInitial(attack);
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
