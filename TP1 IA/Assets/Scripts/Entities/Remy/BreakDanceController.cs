using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakDanceController : MonoBehaviour
{
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("IsBreakDancing", true);
    }
}
