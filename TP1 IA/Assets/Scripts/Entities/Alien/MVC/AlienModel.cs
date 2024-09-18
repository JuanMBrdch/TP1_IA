using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class AlienModel : EnemyModel, IRunAway
{
    [Header("Run Away")]
    [SerializeField] int runAwayDurationTime;

    bool isRunningAway;

    public static Action AttackAction;
    override public Action GetAttackAction { get => AttackAction; }

    public bool IsRunningAway { get => isRunningAway; set => isRunningAway = value; }
    public int RunAwayDuration { get => runAwayDurationTime; set => runAwayDurationTime = value; }

    public void RunAway()
    {
        isRunningAway = true;
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

    protected override void Start()
    {
        base.Start();
        IsRunningAway = false;
    }
}
