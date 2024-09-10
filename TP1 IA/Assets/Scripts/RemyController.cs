using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RemyController : MonoBehaviour
{
    [Header("Parameters")]
    public float speed;
    [SerializeField] float speedLimit = 6;
    [SerializeField] int energyLimit = 2;

    [Header("References")]
    [SerializeField] Animator anim;

    [Header("Ground Check")]
    [SerializeField] LayerMask groundLayer;

    Rigidbody rb;

    bool isBreakDancing;
    bool isJumping;
    bool isGrounded;

    public bool isProtected;

    float raycastDistance = 0.1f; // Distancia del Raycast

    int life;
    int energy = 0;    

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
            if(isJumping) anim.SetTrigger("DoJump");
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

//****************Modified****************
        if (Input.GetKeyDown(KeyCode.Return) && energy > 0) {
            IsBreakDancing = !IsBreakDancing;
            energy--;
            print("Dance");
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            life = 0;
            rb.velocity = Vector3.zero;
            anim.SetTrigger("DoDie");
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

    public void ChangeEnergy(int amount)
    {
        if ((energy + amount) <= energyLimit)
        {
            if ((energy + amount) >= 0)
            {
                energy += amount;                
            }
            else energy = 0;

            print("Energy is now " + energy);
        }
        
    }

    public void ChangeSpeed(float amount)
    {
        if ((speed + amount) <= speedLimit)
        {
            if ((speed + amount) >= 0)
            {
                speed += amount;
            }
            else speed = 0;

            print("Speed is now " + speed);
        }
        else speed = speedLimit;
    }
    
}
