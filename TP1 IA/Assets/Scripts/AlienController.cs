using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class AlienController : MonoBehaviour
{
    [Header("Target")] [SerializeField] Transform target;

    [Header("Properties")] [SerializeField]
    float speed;

    [SerializeField] float pursuitRadius;

    [Header("References")] [SerializeField]
    Animator anim;

    [Header("Obstacle Avoidance")] public float radius;
    public float angle;
    public float personalArea;
    public LayerMask obsMask;
    ObstacleAvoidance _obs;

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

    public bool IsAttacking
    {
        get => isAttacking;
        set => isAttacking = value;
    }

    public float HorizontalVelocityMagnitude
    {
        get { return new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude; }
    }

    private void Awake()
    {
        AlienAnimController.FinishedAttackAction += FinishedAttackActionHandler;
        _obs = new ObstacleAvoidance(transform, radius, angle, personalArea, obsMask);
    }

    private void OnDestroy()
    {
        AlienAnimController.FinishedAttackAction -= FinishedAttackActionHandler;
    }

    void FinishedAttackActionHandler()
    {
        IsAttacking = false;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        isRunningAway = false;
    }

    void Update()
    {
        if (target == null)
            return;

        float targetDistance = (target.position - transform.position).magnitude;

        if (targetDistance <= pursuitRadius && !IsRunningAway && !IsAttacking)
        {
            var dirToTarget = (target.position - transform.position).normalized; //aca deberia ir el pursuit/evade segun corresponda
            dirToTarget.y = 0; //para no elevarlo/bajarlo
            var dir = _obs.GetDir(dirToTarget).normalized; //solo da la direccion a la que moverse
            rb.velocity = dir * speed;
            transform.forward = dir; //hace verlo en la direccion en la que se va a mover
        }
        else
        {
            if (IsRunningAway)
            {
                Vector3 fleeDirection = transform.position - (target.position - transform.position); // mirar para atrás
                transform.LookAt(new Vector3(fleeDirection.x, 0, fleeDirection.z));
                rb.velocity = _obs.GetDir(transform.forward) * speed;
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