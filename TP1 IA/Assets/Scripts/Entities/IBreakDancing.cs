using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBreakDancing
{
    void BreakDance();

    bool CanBreakDance();

    void UpdateCooldowns();

    bool IsBreakDancing { get; set; }
}
