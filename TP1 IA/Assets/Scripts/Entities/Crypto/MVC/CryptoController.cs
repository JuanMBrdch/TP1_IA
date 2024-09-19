using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CryptoController : EnemyController
{
    IClapping _clapping;

    override protected void Start()
    {
        base.Start();
        model = GetComponent<CryptoModel>();
    }

    override protected void InitFSM()
    {
        _idle = GetComponent<IIdle>();
        _move = GetComponent<IMove>();
        _patrol = GetComponent<IPatrol>();
        _attack = GetComponent<IAttack>();
        _clapping = GetComponent<IClapping>();

        fsm = new();

        var idle = new EnemyStateIdle(_idle, _move);
        var patrol = new EnemyStatePatrol(_idle, _move, this.transform, _patrol);
        var pursuit = new EnemyStatePursuit(_move, this.transform, model.target.Rb, model.timePrediction);
        var attack = new EnemyStateAttacking(_move, _attack, this.transform, model.target.Rb, model.timePrediction);
        var clap = new EnemyStateClapping(_move, _clapping); 

        idle.AddTransition(EnemyStates.Patrol, patrol);
        idle.AddTransition(EnemyStates.Pursuit, pursuit);
        idle.AddTransition(EnemyStates.Clap, clap);
        idle.AddTransition(EnemyStates.Attack, attack);

        patrol.AddTransition(EnemyStates.Idle, idle);
        patrol.AddTransition(EnemyStates.Pursuit, pursuit);
        patrol.AddTransition(EnemyStates.Clap, clap);
        patrol.AddTransition(EnemyStates.Attack, attack);

        pursuit.AddTransition(EnemyStates.Idle, idle);
        pursuit.AddTransition(EnemyStates.Patrol, patrol);
        pursuit.AddTransition(EnemyStates.Clap, clap);
        pursuit.AddTransition(EnemyStates.Attack, attack);

        clap.AddTransition(EnemyStates.Idle, idle);
        clap.AddTransition(EnemyStates.Patrol, patrol);
        clap.AddTransition(EnemyStates.Pursuit, pursuit);
        clap.AddTransition(EnemyStates.Attack, attack);

        attack.AddTransition(EnemyStates.Idle, idle);
        attack.AddTransition(EnemyStates.Patrol, patrol);
        attack.AddTransition(EnemyStates.Pursuit, pursuit);
        attack.AddTransition(EnemyStates.Clap, clap);   

        fsm.SetInitial(idle);
    }

    override protected void InitDecisionTree()
    {
        var idle = new ActionTree(() => fsm.Transition(EnemyStates.Idle));
        var patrol = new ActionTree(() => fsm.Transition(EnemyStates.Patrol));
        var pursuit = new ActionTree(() => fsm.Transition(EnemyStates.Pursuit));
        var attack = new ActionTree(() => fsm.Transition(EnemyStates.Attack));
        var clapping = new ActionTree(() => fsm.Transition(EnemyStates.Clap));

        var qCanAttack = new QuestionTree(CanAttack, attack, pursuit);
        var qPlayerBreakDancing = new QuestionTree(IsPlayerBreakDancing, clapping, qCanAttack);
        var qPatrolTime = new QuestionTree(IsPatrolTime, patrol, idle);
        var qIsAlive = new QuestionTree(IsPlayerAlive, qPlayerBreakDancing, idle);
        var qPlayerInSight = new QuestionTree(IsPlayerInSight, qIsAlive, qPatrolTime);
        var qIAmAttacking = new QuestionTree(IAmAttacking, attack, qPlayerInSight);
        var qIAmClapping = new QuestionTree(IAmClapping, clapping, qIAmAttacking);

        actionTreeRoot = qIAmClapping;
    }

    bool IAmClapping()
    {
        return _clapping.IsClapping;
    }
}
