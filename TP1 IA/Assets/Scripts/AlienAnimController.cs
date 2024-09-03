using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienAnimController : MonoBehaviour
{
    public static Action FinishedAttackAction;

    public bool FinishedAttacking()
    {
        FinishedAttackAction?.Invoke();
        return true;
    }
}
