using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemyAnimController : MonoBehaviour
{

    public static Action FinishedJumpAction;

    public bool FinishedJumping()
    {
        FinishedJumpAction?.Invoke();
        return true;
    }
}
