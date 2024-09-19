using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyModel : Enemy
{
    [Header("Line of sight grace time")]
    [SerializeField] float lineOfSightGraceTime;
    public abstract Action GetAttackAction { get; }
    public float LineOfSightGraceTime { get => lineOfSightGraceTime; set => lineOfSightGraceTime = value; }

    protected override void Start()
    {
        base.Start();
    }

    override public void Attack()
    {
        base.Attack();
        GetAttackAction?.Invoke();
    }
}
