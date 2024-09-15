using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CryptoStateAttacking : State<CryptoStates>
{
    Transform target;

    IMove move;
    IAttack attack;

    public CryptoStateAttacking(IMove move, IAttack attack, Transform target)
    {
        this.move = move;
        this.attack = attack;
        this.target = target;
    }
    public override void Enter()
    {
        base.Enter();
        move.Move(Vector3.zero);
        attack.Attack();
    }

    public override void FixedExecute()
    {
        base.FixedExecute();
        move.Look(target);
    }
}
