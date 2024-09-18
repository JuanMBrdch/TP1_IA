using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{
    protected FSM<EnemyStates> fsm;
    protected ITreeNode actionTreeRoot;
    protected LineOfSight lineOfSight;

    protected IIdle _idle;
    protected IMove _move;
    protected IPatrol _patrol;
    protected IAttack _attack;

    protected EnemyModel model;

    protected Cooldown graceTimeCooldown;

    private void Awake()
    {
        lineOfSight = GetComponent<LineOfSight>();
        model = GetComponent<EnemyModel>();
        graceTimeCooldown = new Cooldown(model.LineOfSightGraceTime);
    }

    protected virtual void Start()
    {
        InitFSM();
        InitDecisionTree();
    }
    protected virtual void InitFSM()
    {
        _idle = GetComponent<IIdle>();
        _move = GetComponent<IMove>();
        _patrol = GetComponent<IPatrol>();
        _attack = GetComponent<IAttack>();
    }

    protected abstract void InitDecisionTree();

    protected bool IAmAttacking()
    {
        return _attack.IsAttacking;
    }

    protected bool IsPlayerInSight()
    {
        bool InSightAndInRangeAndWithinAngle =
            lineOfSight.InView(model.target.transform) &&
            lineOfSight.CheckRange(model.target.transform) &&
            lineOfSight.CheckAngle(model.target.transform);

        if (InSightAndInRangeAndWithinAngle)
            graceTimeCooldown.ResetCooldown();

        return graceTimeCooldown.IsCooldown() || InSightAndInRangeAndWithinAngle;
    }

    protected bool IsPlayerAlive()
    {
        RemyModel remyModel = model.target.GetComponent<RemyModel>();
        if (remyModel != null)
            return remyModel.IsAlive;

        return false;
    }

    protected bool IsPatrolTime()
    {
        return !_idle.IsIdleing;
    }

    protected bool IsPlayerBreakDancing()
    {
        RemyModel remyModel = model.target.GetComponent<RemyModel>();
        if (remyModel != null)
            return remyModel.IsBreakDancing;

        return false;
    }
    protected bool CanAttack()
    {
        return (model.target.transform.position - transform.position).magnitude <= _attack.AttackRange;
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
