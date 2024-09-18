using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IIdle
{
    public bool IsIdleing { get; set; }
    public float WaitingTime { get; }
}
