using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class CryptoStateAttacking : State<CryptoStates>
{
    IMove move;
    IAttack attack;

    Cooldown attackCooldown;
    
    Pursuit pursuit;

    public CryptoStateAttacking(IMove move, IAttack attack, Transform entity, Rigidbody target, float timePrediction)
    {
        this.move = move;
        this.attack = attack;

        attackCooldown = new Cooldown(attack.AttackCooldown);
        pursuit = new(entity, target, timePrediction * .5f);
    }
    public override void Enter()
    {
        base.Enter();
        move.Move(Vector3.zero);
        Attack();
    }

    public override void FixedExecute()
    {
        base.FixedExecute();

        Vector3 pursuitDir = pursuit.GetDir();
        move.Look(pursuitDir);

        if (!attack.IsAttacking && !attackCooldown.IsCooldown()) // Si estoy atacando, y ya terminé de atacar, vuelvo a atacar
            Attack();
    }

    private void Attack()
    {
        attack.Attack();
        attackCooldown.ResetCooldown();
    }
}
