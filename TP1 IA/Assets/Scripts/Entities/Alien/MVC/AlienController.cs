using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class AlienController : MonoBehaviour
{
    FSM<EnemyStates> fsm;
    ITreeNode actionTreeRoot;
    LineOfSight lineOfSight;

    IIdle _idle;
    IMove _move;
    IPatrol _patrol;
    IRunAway _runningAway;
    IAttack _attack;

    AlienModel alienModel;

    Cooldown graceTimeCooldown;

    private void Awake()
    {
        lineOfSight = GetComponent<LineOfSight>();
        alienModel = GetComponent<AlienModel>();

        graceTimeCooldown = new Cooldown(alienModel.LineOfSightGraceTime);
    }

    void Start()
    {
        InitFSM();
        InitDecisionTree();
    }
    void InitFSM()
    {
        _idle = GetComponent<IIdle>();
        _move = GetComponent<IMove>();
        _patrol = GetComponent<IPatrol>();
        _attack = GetComponent<IAttack>();
        _runningAway = GetComponent<IRunAway>();

        fsm = new();

        var idle = new EnemyStateIdle(_idle, _move);
        var patrol = new EnemyStatePatrol(_move, this.transform, _patrol);
        var pursuit = new EnemyStatePursuit(_move, this.transform, alienModel.target.Rb, alienModel.timePrediction);
        var attack = new EnemyStateAttacking(_move, _attack, this.transform, alienModel.target.Rb, alienModel.timePrediction);
        var runAway = new EnemyStateRunningAway(_move, _runningAway);

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

    void InitDecisionTree()
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
    bool CanAttack()
    {
        return (alienModel.target.transform.position - transform.position).magnitude <= _attack.AttackRange;
    }
    bool IsPlayerBreakDancing()
    {
        RemyModel remyModel = alienModel.target.GetComponent<RemyModel>();
        if (remyModel != null)
            return remyModel.IsBreakDancing;

        return false;
    }

    bool IsPatrolTime()
    {
        // TODO: Implement Patrol
        return false;
    }

    bool IsPlayerAlive()
    {
        RemyModel remyModel = alienModel.target.GetComponent<RemyModel>();
        if (remyModel != null)
            return remyModel.IsAlive;

        return false;
    }

    bool IsPlayerInSight()
    {
        bool InSightAndInRangeAndWithinAngle = 
            lineOfSight.InView(alienModel.target.transform) &&
            lineOfSight.CheckRange(alienModel.target.transform) &&
            lineOfSight.CheckAngle(alienModel.target.transform);

        if (InSightAndInRangeAndWithinAngle)
            graceTimeCooldown.ResetCooldown();

        return graceTimeCooldown.IsCooldown() || InSightAndInRangeAndWithinAngle;
    }
    bool IAmRunningAway()
    {
        return _runningAway.IsRunningAway;
    }

    bool IAmAttacking()
    {
        return _attack.IsAttacking;
    }

    void Update()
    {
        fsm.OnUpdate();
        actionTreeRoot.Execute();

        print(fsm.GetCurrent);
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
