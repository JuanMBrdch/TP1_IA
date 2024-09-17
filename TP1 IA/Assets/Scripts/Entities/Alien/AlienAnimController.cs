using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienAnimController : MonoBehaviour
{
    public static Action FinishedAttackAction;
    public static Action ConcreteAttackAction;
    public bool FinishedAttacking()
    {
        FinishedAttackAction?.Invoke();
        return true;
    }
    public void ConcreteAttack()
    {
        ConcreteAttackAction?.Invoke();
    }
}
