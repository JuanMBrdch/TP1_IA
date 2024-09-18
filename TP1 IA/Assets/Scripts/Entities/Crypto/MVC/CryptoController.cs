using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CryptoController : MonoBehaviour
{
    FSM<EnemyStates> fsm;
    ITreeNode actionTreeRoot;
    LineOfSight lineOfSight;

    IMove _move;
    IPatrol _patrol;
    IAttack _attack;
    IClapping _clapping;

    CryptoModel cryptoModel;

    Cooldown graceTimeCooldown;

    private void Awake()
    {
        lineOfSight = GetComponent<LineOfSight>();
        cryptoModel = GetComponent<CryptoModel>();

        graceTimeCooldown = new Cooldown(cryptoModel.LineOfSightGraceTime);
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
        _attack = GetComponent<IAttack>();
        _clapping = GetComponent<IClapping>();

        fsm = new();

        var idle = new EnemyStateIdle(_move);
        var patrol = new EnemyStatePatrol(_move, this.transform, _patrol);
        var pursuit = new EnemyStatePursuit(_move, this.transform, cryptoModel.target.Rb, cryptoModel.timePrediction);
        var attack = new EnemyStateAttacking(_move, _attack, this.transform, cryptoModel.target.Rb, cryptoModel.timePrediction);
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

    void InitDecisionTree()
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

    bool IAmAttacking()
    {
        return _attack.IsAttacking;
    }

    bool IsPlayerInSight()
    {
        bool InSightAndInRangeAndWithinAngle =
            lineOfSight.InView(cryptoModel.target.transform) &&
            lineOfSight.CheckRange(cryptoModel.target.transform) &&
            lineOfSight.CheckAngle(cryptoModel.target.transform);

        if (InSightAndInRangeAndWithinAngle)
            graceTimeCooldown.ResetCooldown();

        return graceTimeCooldown.IsCooldown() || InSightAndInRangeAndWithinAngle;
    }

    bool IsPlayerAlive()
    {
        RemyModel remyModel = cryptoModel.target.GetComponent<RemyModel>();
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
        RemyModel remyModel = cryptoModel.target.GetComponent<RemyModel>();
        if (remyModel != null)
            return remyModel.IsBreakDancing;

        return false;
    }

    bool CanAttack()
    {
        return (cryptoModel.target.transform.position - transform.position).magnitude <= _attack.AttackRange;
    }

    void Update()
    {
        fsm.OnUpdate();
        actionTreeRoot.Execute();
    }
    void FixedUpdate()
    {
        fsm.OnFixedUpdate();
    }
    void LateUpdate()
    {
        fsm.OnLateUpdate();
    }
}
