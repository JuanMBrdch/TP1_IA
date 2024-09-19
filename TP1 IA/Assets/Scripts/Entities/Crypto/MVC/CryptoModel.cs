using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryptoModel : EnemyModel, IClapping
{
    [Header("Clapping")]
    [SerializeField] int clappingDurationTime;

    bool isClapping;

    public bool IsClapping { get => isClapping; set => isClapping = value; }
    public int ClappingDuration { get => clappingDurationTime; set => clappingDurationTime = value; }

    public static Action AttackAction;
    override public Action GetAttackAction { get => AttackAction; }

    protected override void Start()
    {
        base.Start();
        IsClapping = false;
    }

    override protected void Awake()
    {
        base.Awake();
        CryptoAnimController.FinishedAttackAction += FinishedAttackActionHandler;
        CryptoAnimController.ConcreteAttackAction += ConcreteAttackActionHandler;
    }

    override protected void OnDestroy()
    {
        CryptoAnimController.FinishedAttackAction -= FinishedAttackActionHandler;
        CryptoAnimController.ConcreteAttackAction -= ConcreteAttackActionHandler;
    }

    public void Clap()
    {
        IsClapping = true;
    }
}
