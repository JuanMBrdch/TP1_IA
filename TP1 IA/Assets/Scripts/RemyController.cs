using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RemyController : MonoBehaviour
{
    [Header("Parameters")]
    public float speed;

    [Header("References")]
    [SerializeField] Animator anim;

    [Header("Ground Check")]
    [SerializeField] LayerMask groundLayer;

    Rigidbody rb;

    bool isBreakDancing;
    bool isJumping;
    bool isGrounded;

    float raycastDistance = 0.1f; // Distancia del Raycast

    int life;

    public bool IsBreakDancing { 
        get => isBreakDancing; 
        set { 
            isBreakDancing = value;
            anim.SetBool("IsBreakDancing", isBreakDancing);
        }
    }

    public bool IsJumping { 
        get => isJumping;
        set { 
            isJumping = value;
            if(isJumping) anim.SetTrigger("IsJumping");
        }
    }

    public bool IsAlive
    {
        get => life > 0;
    }
    public float HorizontalVelocityMagnitude
    {
        get
        {
            return new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;
        }
    }

    private void Awake()
    {
        RemyAnimController.FinishedJumpAction += FinishedJumpActionHandler;
    }

    private void OnDestroy()
    {
        RemyAnimController.FinishedJumpAction -= FinishedJumpActionHandler;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        IsBreakDancing = false;
        IsJumping = false;
        life = 10;
    }

    void FinishedJumpActionHandler()
    {
        IsJumping = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsAlive)
            return;
        
        // Raycast desde el centro del objeto hacia abajo
        isGrounded = Physics.Raycast(transform.position, Vector3.down, raycastDistance, groundLayer);

        // Opcional: Visualizar el Raycast en la escena
        Debug.DrawRay(transform.position, Vector3.down * raycastDistance, Color.red);

        if (isGrounded && !IsJumping && Input.GetKeyDown(KeyCode.Space))
        {
            IsJumping = true;
            rb.AddForce(Vector3.up * 6, ForceMode.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.Return)) {
            IsBreakDancing = !IsBreakDancing;
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            life = 0;
            rb.velocity = Vector3.zero;
            anim.SetTrigger("IsDying");
            return;
        }

        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");

        /* 
            Forma cabezona para que tenga el smooth del GetAxis 
            pero que al moverse en diagonal no se mueva mas rapido.
        */
        if (h != 0 && v != 0) 
        {
            h *= 0.70710678f;
            v *= 0.70710678f;
        }

        Vector3 dir = new Vector3(h, 0, v);

        dir *= speed;
        dir.y = rb.velocity.y;
        rb.velocity = dir;

        if (h != 0 || v != 0)
        {
            transform.forward = new Vector3(dir.x, 0, dir.z);
        }

        anim.SetFloat("Velocity", HorizontalVelocityMagnitude);
    }
}
