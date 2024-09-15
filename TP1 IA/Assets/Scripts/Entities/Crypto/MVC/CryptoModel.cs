using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryptoModel : Enemy, IClapping, IAttack
{
    [SerializeField] GameObject fireball;
    [SerializeField] Transform attackSpawnPoint;

    bool isClapping;

    public static Action AttackAction;
    public bool IsClapping
    {
        get => isClapping;
        set => isClapping = value;
    }

    protected override void Start()
    {
        base.Start();
        IsClapping = false;
        IsAttacking = false;
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
        newFireball.GetComponent<FireballController>().Direction = transform.forward;
    }
}
