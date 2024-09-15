using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemyView : MonoBehaviour
{
    [SerializeField] Animator anim;
    Rigidbody rb;

    IJump _jump;
    IBreakDancing _breakDancing;
    IDead _dead;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _jump = GetComponent<IJump>();
        _breakDancing = GetComponent<IBreakDancing>();
        _dead = GetComponent<IDead>();

        RemyModel.DieAction += DieActionHandler;
        RemyModel.JumpAction += JumpActionHandler;
    }

    private void OnDestroy()
    {
        RemyModel.DieAction -= DieActionHandler;
        RemyModel.JumpAction -= JumpActionHandler;
    }

    // Update is called once per frame
    void Update()
    {
        if (_dead.IsDead())
            return;

        anim.SetFloat("Velocity", new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude);
        anim.SetBool("IsBreakDancing", _breakDancing.IsBreakDancing);
    }

    void DieActionHandler()
    {
        anim.SetTrigger("DoDie");
    }

    void JumpActionHandler()
    {
        anim.SetTrigger("DoJump");
    }
}
