using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienStateAttacking : State<AlienStates>
{
    Transform target;

    IMove move;
    IAttack attack;

    Cooldown attackingCooldown;

    public AlienStateAttacking(IMove move, IAttack attack, Transform target)
    {
        this.move = move;
        this.attack = attack;
        this.target = target;

        attackingCooldown = new Cooldown(attack.AttackCooldown);
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

        if (!attack.IsAttacking && !attackingCooldown.IsCooldown()) // Si estoy atacando, y ya terminé de atacar, vuelvo a atacar
            Attack();
    }

    private void Attack()
    {
        attack.Attack();
        attackingCooldown.ResetCooldown();
    }
}
