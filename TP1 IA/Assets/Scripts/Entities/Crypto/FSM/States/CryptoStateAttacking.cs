using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CryptoStateAttacking : State<CryptoStates>
{
    Transform target;

    IMove move;
    IAttack attack;

    Cooldown clappingCooldownDuration;

    public CryptoStateAttacking(IMove move, IAttack attack, Transform target)
    {
        this.move = move;
        this.attack = attack;
        this.target = target;

        clappingCooldownDuration = new Cooldown(attack.AttackCooldown);
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
        move.Look(target);

        if (!attack.IsAttacking && !clappingCooldownDuration.IsCooldown()) // Si estoy atacando, y ya terminé de atacar, vuelvo a atacar
            Attack();
    }

    private void Attack()
    {
        attack.Attack();
        clappingCooldownDuration.ResetCooldown();
    }
}
