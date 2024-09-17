using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryptoModel : Enemy, IClapping
{
    [Header("Attack")]
    [SerializeField] GameObject fireball;
    [SerializeField] Transform attackSpawnPoint;

    [Header("Clapping")]
    [SerializeField] int clappingDurationTime;

    bool isClapping;

    public static Action AttackAction;
    public bool IsClapping
    {
        get => isClapping;
        set => isClapping = value;
    }
    public int ClappingDuration { get => clappingDurationTime; set => clappingDurationTime = value; }

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

    override public void Attack()
    {
        base.Attack();
        AttackAction?.Invoke();   
    }

    override protected void ConcreteAttackActionHandler()
    {
        GameObject newFireball = Instantiate(fireball, attackSpawnPoint.position, Quaternion.identity);
        newFireball.GetComponent<Attack>().Direction = transform.forward;
    }
}
