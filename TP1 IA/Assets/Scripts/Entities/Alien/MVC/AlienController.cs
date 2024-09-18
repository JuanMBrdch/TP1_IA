using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class AlienController : EnemyController
{
    IRunAway _runningAway;

    override protected void Start()
    {
        base.Start();
        model = GetComponent<AlienModel>();
    }
    override protected void InitFSM()
    {
        base.InitFSM();
        _runningAway = GetComponent<IRunAway>();

        fsm = new();

        var idle = new EnemyStateIdle(_idle, _move);
        var patrol = new EnemyStatePatrol(_idle, _move, this.transform, _patrol);
        var pursuit = new EnemyStatePursuit(_move, this.transform, model.target.Rb, model.timePrediction);
        var attack = new EnemyStateAttacking(_move, _attack, this.transform, model.target.Rb, model.timePrediction);
        var runAway = new EnemyStateRunningAway(_move, _runningAway, this.transform, model.target.Rb, model.timePrediction);

        idle.AddTransition(EnemyStates.Patrol, patrol);
        idle.AddTransition(EnemyStates.Pursuit, pursuit);
        idle.AddTransition(EnemyStates.RunAway, runAway);
        idle.AddTransition(EnemyStates.Attack, attack);

        patrol.AddTransition(EnemyStates.Idle, idle);
        patrol.AddTransition(EnemyStates.Pursuit, pursuit);
        patrol.AddTransition(EnemyStates.RunAway, runAway);
        patrol.AddTransition(EnemyStates.Attack, attack);

        pursuit.AddTransition(EnemyStates.Idle, idle);
        pursuit.AddTransition(EnemyStates.Patrol, patrol);
        pursuit.AddTransition(EnemyStates.RunAway, runAway);
        pursuit.AddTransition(EnemyStates.Attack, attack);

        runAway.AddTransition(EnemyStates.Idle, idle);
        runAway.AddTransition(EnemyStates.Patrol, patrol);
        runAway.AddTransition(EnemyStates.Pursuit, pursuit);
        runAway.AddTransition(EnemyStates.Attack, attack);

        attack.AddTransition(EnemyStates.Idle, idle);
        attack.AddTransition(EnemyStates.Patrol, patrol);
        attack.AddTransition(EnemyStates.Pursuit, pursuit);
        attack.AddTransition(EnemyStates.RunAway, runAway);

        fsm.SetInitial(idle);
    }

    override protected void InitDecisionTree()
    {
        var idle = new ActionTree(() => fsm.Transition(EnemyStates.Idle));
        var patrol = new ActionTree(() => fsm.Transition(EnemyStates.Patrol));
        var pursuit = new ActionTree(() => fsm.Transition(EnemyStates.Pursuit));
        var attack = new ActionTree(() => fsm.Transition(EnemyStates.Attack));
        var runningAway = new ActionTree(() => fsm.Transition(EnemyStates.RunAway));

        var qCanAttack = new QuestionTree(CanAttack, attack, pursuit);
        var qPlayerBreakDancing = new QuestionTree(IsPlayerBreakDancing, runningAway, qCanAttack);
        var qPatrolTime = new QuestionTree(IsPatrolTime, patrol, idle);
        var qIsAlive = new QuestionTree(IsPlayerAlive, qPlayerBreakDancing, idle);
        var qPlayerInSight = new QuestionTree(IsPlayerInSight, qIsAlive, qPatrolTime);
        var qIAmAttacking = new QuestionTree(IAmAttacking, attack, qPlayerInSight);
        var qIAmRunningAway = new QuestionTree(IAmRunningAway, runningAway, qIAmAttacking);

        actionTreeRoot = qIAmRunningAway;
    }
    bool IAmRunningAway()
    {
        return _runningAway.IsRunningAway;
    }
}
