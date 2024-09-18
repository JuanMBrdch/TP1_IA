using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class AlienModel : Enemy, IRunAway
{
    [Header("Attack")]
    [SerializeField] GameObject meleeAttack;
    [SerializeField] Transform attackSpawnPoint;

    [Header("Run Away")]
    [SerializeField] int runAwayDurationTime;

    public static Action AttackAction;

    bool isRunningAway;

    public bool IsRunningAway { get => isRunningAway; set => isRunningAway = value; }
    public int RunAwayDuration { get => runAwayDurationTime; set => runAwayDurationTime = value; }

    public void RunAway()
    {
        isRunningAway = true;
        // TODO: Implement Runaway
    }

    override public void Attack()
    {
        base.Attack();
        AttackAction?.Invoke();
    }

    override protected void Awake()
    {
        base.Awake();
        AlienAnimController.FinishedAttackAction += FinishedAttackActionHandler;
        AlienAnimController.ConcreteAttackAction += ConcreteAttackActionHandler;
    }
    override protected void OnDestroy()
    {
        AlienAnimController.FinishedAttackAction -= FinishedAttackActionHandler;
        AlienAnimController.ConcreteAttackAction -= ConcreteAttackActionHandler;
    }

    protected override void ConcreteAttackActionHandler()
    {
        GameObject newFireball = Instantiate(meleeAttack, attackSpawnPoint.position, Quaternion.identity);
        newFireball.GetComponent<Attack>().Direction = transform.forward;
    }

    protected override void Start()
    {
        base.Start();
        IsRunningAway = false;
    }
}
