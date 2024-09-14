using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryptoController : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] Transform target;

    [Header("Properties")]
    [SerializeField] float speed;
    [SerializeField] float pursuitRadius;

    [Header("References")]
    [SerializeField] Animator anim;

    [Header("Obstacle Avoidance")]
    public float radius;
    public float angle;
    public float personalArea;
    public LayerMask obsMask;
    ObstacleAvoidance _obs;
    
    // izi: en el move pasarle la direccion del OA para moverse siempre con esto, problema: siempre lo usa
    // hard: pasarle el OA a los estados, y de ahi usar la direccion en el move (referenciarlos y pasarlos por constructor)
    // recomm: rotacion del jugador suave
    
    Rigidbody rb;
    bool isClapping;
    bool isAttacking;

    public bool IsClapping
    {
        get => isClapping;
        set
        {
            isClapping = value;
            anim.SetBool("IsClapping", isClapping);
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
        CryptoAnimController.FinishedAttackAction += FinishedAttackActionHandler;
        _obs = new ObstacleAvoidance(transform, radius, angle, personalArea, obsMask);
    }

    private void OnDestroy()
    {
        CryptoAnimController.FinishedAttackAction -= FinishedAttackActionHandler;
    }

    void FinishedAttackActionHandler()
    {
        IsAttacking = false;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        IsClapping = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
            return;

        float targetDistance = (target.position - transform.position).magnitude;

        if (targetDistance <= pursuitRadius && !IsClapping && !IsAttacking)
        {
            transform.LookAt(new Vector3(target.position.x, 0, target.position.z));
            rb.velocity = new Vector3(transform.forward.x, 0, transform.forward.z) * speed;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }

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

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, pursuitRadius);
    }
#endif
}
