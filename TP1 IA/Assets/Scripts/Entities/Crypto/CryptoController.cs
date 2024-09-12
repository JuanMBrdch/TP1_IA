using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryptoController : Enemy
{
    bool isClapping;

    public bool IsClapping
    {
        get => isClapping;
        set
        {
            isClapping = value;
            anim.SetBool("IsClapping", isClapping);
        }
    }

    override protected void Awake()
    {
        CryptoAnimController.FinishedAttackAction += FinishedAttackActionHandler;
    }

    override protected void OnDestroy()
    {
        CryptoAnimController.FinishedAttackAction -= FinishedAttackActionHandler;
    }

    protected override void Start()
    {
        base.Start();
        IsClapping = false;
        IsAttacking = false;
    }

    // Update is called once per frame
     void Update()
    {
        if (target == null)
            return;

        anim.SetFloat("Velocity", HorizontalVelocityMagnitude);

        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            IsClapping = !IsClapping;
        }

        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            IsAttacking = true;
            anim.SetTrigger("DoAttack");
        }
    }

    /*
    protected override void Move()
    {
        if (IsTargetCloseEnough && !IsClapping && !IsAttacking)
            rb.velocity = new Vector3(transform.forward.x, 0, transform.forward.z) * speed;
        else
            rb.velocity = Vector3.zero;
    }

    protected override void LookAt()
    {
        if (IsTargetCloseEnough && !IsClapping && !IsAttacking)
            transform.LookAt(new Vector3(target.position.x, 0, target.position.z));
    }
    */
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, pursuitRadius);
    }
#endif
}
