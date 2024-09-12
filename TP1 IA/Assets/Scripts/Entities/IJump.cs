using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IJump
{
    void Jump();
    bool CanJump();
    bool IsJumping { get; set; }
}
