using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class AlienController : Enemy
{
    bool isRunningAway;

    public bool IsRunningAway
    { 
        get => isRunningAway;
        set
        {
            isRunningAway = value;
            anim.SetBool("IsRunningAway", isRunningAway);
        }
    }

    override protected void Awake()
    {
        AlienAnimController.FinishedAttackAction += FinishedAttackActionHandler;
    }

    override protected void OnDestroy()
    {
        AlienAnimController.FinishedAttackAction -= FinishedAttackActionHandler;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        IsRunningAway = false;
        IsAttacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
            return;

        anim.SetFloat("Velocity", HorizontalVelocityMagnitude);

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            IsRunningAway = !IsRunningAway;
        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            IsAttacking = true;
            anim.SetTrigger("DoAttack");
        }
    }
    /*
    protected override void Move()
    {
        if (IsTargetCloseEnough && !IsRunningAway && !IsAttacking)
        {
            rb.velocity = new Vector3(transform.forward.x, 0, transform.forward.z) * speed;
        }
        else
        {
            if (IsRunningAway)
            {
                
                rb.velocity = new Vector3(transform.forward.x, 0, transform.forward.z) * speed;
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        }
    }

    protected override void LookAt()
    {
        if (IsTargetCloseEnough && !IsRunningAway && !IsAttacking)
        {
            transform.LookAt(new Vector3(target.position.x, 0, target.position.z));
        }
        else
        {
            if (IsRunningAway)
            {
                Vector3 fleeDirection = transform.position - (target.position - transform.position); // mirar para atrás
                transform.LookAt(new Vector3(fleeDirection.x, 0, fleeDirection.z));
            }
        }
    }
    */
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, pursuitRadius);
    }
#endif
}
