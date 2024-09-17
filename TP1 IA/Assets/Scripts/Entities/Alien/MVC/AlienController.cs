using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class AlienController : MonoBehaviour
{
    public Transform target;

    FSM<AlienStates> fsm;
    ITreeNode actionTreeRoot;
    LineOfSight lineOfSight;

    IMove _move;
    IPatrol _patrol;
    IRunAway _runningAway;
    IAttack _attack;

    private void Awake()
    {
        lineOfSight = GetComponent<LineOfSight>();
    }

    void Start()
    {
        InitFSM();
        InitDecisionTree();
    }
    void InitFSM()
    {
        _move = GetComponent<IMove>();
        _patrol = GetComponent<IPatrol>();
        _runningAway = GetComponent<IRunAway>();
        _attack = GetComponent<IAttack>();

        fsm = new();

        var idle = new AlienStateIdle(_move);
        var patrol = new AlienStatePatrol(_move, this.transform, _patrol);
        var pursuit = new AlienStatePursuit(_move, this.transform, target);
        var runAway = new AlienStateRunningAway(_move, _runningAway);
        var attack = new AlienStateAttacking(_move, _attack, target);

        idle.AddTransition(AlienStates.Patrol, patrol);
        idle.AddTransition(AlienStates.Pursuit, pursuit);
        idle.AddTransition(AlienStates.RunAway, runAway);
        idle.AddTransition(AlienStates.Attack, attack);

        patrol.AddTransition(AlienStates.Idle, idle);
        patrol.AddTransition(AlienStates.Pursuit, pursuit);
        patrol.AddTransition(AlienStates.RunAway, runAway);
        patrol.AddTransition(AlienStates.Attack, attack);

        pursuit.AddTransition(AlienStates.Idle, idle);
        pursuit.AddTransition(AlienStates.Patrol, patrol);
        pursuit.AddTransition(AlienStates.RunAway, runAway);
        pursuit.AddTransition(AlienStates.Attack, attack);

        runAway.AddTransition(AlienStates.Idle, idle);
        runAway.AddTransition(AlienStates.Patrol, patrol);
        runAway.AddTransition(AlienStates.Pursuit, pursuit);
        runAway.AddTransition(AlienStates.Attack, attack);

        attack.AddTransition(AlienStates.Idle, idle);
        attack.AddTransition(AlienStates.Patrol, patrol);
        attack.AddTransition(AlienStates.Pursuit, pursuit);
        attack.AddTransition(AlienStates.RunAway, runAway);

        fsm.SetInitial(idle);
    }

    void InitDecisionTree()
    {
        var idle = new ActionTree(() => fsm.Transition(AlienStates.Idle));
        var patrol = new ActionTree(() => fsm.Transition(AlienStates.Patrol));
        var pursuit = new ActionTree(() => fsm.Transition(AlienStates.Pursuit));
        var attack = new ActionTree(() => fsm.Transition(AlienStates.Attack));
        var runningAway = new ActionTree(() => fsm.Transition(AlienStates.RunAway));

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
        return (target.position - transform.position).magnitude <= _attack.AttackRange;
    }
    bool IsPlayerBreakDancing()
    {
        RemyModel remyModel = target.GetComponent<RemyModel>();
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
        RemyModel remyModel = target.GetComponent<RemyModel>();
        if (remyModel != null)
            return remyModel.IsAlive;

        return false;
    }

    bool IsPlayerInSight()
    {
        return lineOfSight.InView(target) && lineOfSight.CheckRange(target) && lineOfSight.CheckAngle(target);
    }

    bool IAmAttacking()
    {
        return _attack.IsAttacking;
    }
    bool IAmRunningAway()
    {
        return _runningAway.IsRunningAway;
    }

    void Update()
    {
        fsm.OnUpdate();
        actionTreeRoot.Execute();
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
