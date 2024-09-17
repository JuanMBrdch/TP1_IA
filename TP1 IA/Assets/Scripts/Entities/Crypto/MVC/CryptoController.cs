using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CryptoController : MonoBehaviour
{
    public Transform target;

    FSM<CryptoStates> fsm;
    ITreeNode actionTreeRoot;

    IMove _move;
    IPatrol _patrol;
    IClapping _clapping;
    IAttack _attack;

    LineOfSight lineOfSight;

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

        fsm.SetInitial(idle);
    }

    void InitDecisionTree()
    {
        var idle = new ActionTree(() => fsm.Transition(CryptoStates.Idle));
        var patrol = new ActionTree(() => fsm.Transition(CryptoStates.Patrol));
        var pursuit = new ActionTree(() => fsm.Transition(CryptoStates.Pursuit));
        var attack = new ActionTree(() => fsm.Transition(CryptoStates.Attack));
        var clapping = new ActionTree(() => fsm.Transition(CryptoStates.Clap));

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

    bool IAmAttacking()
    {
        return _attack.IsAttacking;
    }


    bool IsPlayerInSight()
    {
        return lineOfSight.InView(target) && lineOfSight.CheckRange(target) && lineOfSight.CheckAngle(target);
    }

    bool IsPlayerAlive()
    {
        RemyModel remyModel = target.GetComponent<RemyModel>();
        if (remyModel != null)
            return remyModel.IsAlive;

        return false;
    }

    bool IsPatrolTime()
    {
        // TODO: Implement Patrol
        return false;
    }

    bool IsPlayerBreakDancing()
    {
        RemyModel remyModel = target.GetComponent<RemyModel>();
        if (remyModel != null)
            return remyModel.IsBreakDancing;

        return false;
    }

    bool CanAttack()
    {
        return (target.position - transform.position).magnitude <= _attack.AttackRange;
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
