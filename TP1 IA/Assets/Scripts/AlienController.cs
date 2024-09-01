using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class AlienController : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] Transform target;

    [Header("Properties")]
    [SerializeField] float speed;
    [SerializeField] float pursuitRadius;

    [Header("References")]
    [SerializeField] Animator anim;

    Rigidbody rb;
    bool isRunningAway;
    bool isAttacking;

    public bool IsRunningAway
    { 
        get => isRunningAway;
        set
        {
            isRunningAway = value;
            anim.SetBool("IsRunningAway", isRunningAway);
        }
    }
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }

    public float HorizontalVelocityMagnitude
    {
        get
        {
            return new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;
        }
    }

    private void Awake()
    {
        AlienAnimController.FinishedAttackAction += FinishedAttackActionHandler;
    }

    private void OnDestroy()
    {
        AlienAnimController.FinishedAttackAction -= FinishedAttackActionHandler;
    }

    void FinishedAttackActionHandler()
    {
        IsAttacking = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        isRunningAway = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
            return;

        float targetDistance = (target.position - transform.position).magnitude;

        if (targetDistance <= pursuitRadius && !IsRunningAway && !IsAttacking)
        {
            transform.LookAt(new Vector3(target.position.x, 0, target.position.z));
            rb.velocity = new Vector3(transform.forward.x, 0, transform.forward.z) * speed;
        }
        else 
        {
            if (IsRunningAway)
            {
                Vector3 fleeDirection = transform.position - (target.position - transform.position); // mirar para atrás
                transform.LookAt(new Vector3(fleeDirection.x, 0, fleeDirection.z));
                rb.velocity = new Vector3(transform.forward.x, 0, transform.forward.z) * speed;
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        }

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

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, pursuitRadius);
    }
#endif
}
