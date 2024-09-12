using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class RemyController : Entity, IJump, IBreakDancing, IDead
{
    [Header("Ground Check")]
    [SerializeField] LayerMask groundLayer;

    [Header("Parameters")]
    [SerializeField] float speedLimit = 6;
    [SerializeField] int energyLimit = 2;
    [SerializeField] int breakDancingDurationTime = 3;
    [SerializeField] int breakDancingCoolDownTime = 5;
    [SerializeField] int energy;
    [SerializeField] int life;

    bool isBreakDancing;
    bool isJumping;
    bool isGrounded;
    bool isProtected;

    readonly float raycastDistance = 0.1f; // Distancia del Raycast

    Cooldown breakDancingCoolDown;
    Cooldown breakDancingDuration;

    IMove _move;
    IJump _jump;
    IBreakDancing _breakDancing;
    IDead _dead;

    public bool IsBreakDancing { 
        get {
            return isBreakDancing;
        }
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
    public bool IsProtected { get => isProtected; set => isProtected = value; }

    FSM<RemyStates> fsm;

    private void Awake()
    {
        RemyAnimController.FinishedJumpAction += FinishedJumpActionHandler;
    }

    private void OnDestroy()
    {
        RemyAnimController.FinishedJumpAction -= FinishedJumpActionHandler;
    }
    void FinishedJumpActionHandler()
    {
        IsJumping = false;
    }

    protected override void Start()
    {
        base.Start();

        InitFSM();

        IsBreakDancing = false;
        IsJumping = false;

        breakDancingDuration = new(breakDancingDurationTime, FinishBrakeDance);
        breakDancingCoolDown = new(breakDancingCoolDownTime);
    }
    void InitFSM()
    {
        _move = GetComponent<IMove>();
        _jump = GetComponent<IJump>();
        _breakDancing = GetComponent<IBreakDancing>();
        _dead = GetComponent<IDead>();

        fsm = new FSM<RemyStates>();

        var idle = new RemyStateIdle(fsm, _move, _jump, _breakDancing, _dead);
        var move = new RemyStateMove(fsm, _move, _jump, _breakDancing, _dead);
        var jump = new RemyStateJump(fsm, _move, _jump);
        var breakDancing = new RemyStateBreakDancing(fsm, _move, _jump, _breakDancing);
        var dead = new RemyStateDead(fsm, _move, _dead);

        idle.AddTransition(RemyStates.Move, move);
        idle.AddTransition(RemyStates.Jump, jump);
        idle.AddTransition(RemyStates.BreakDancing, breakDancing);
        idle.AddTransition(RemyStates.Dead, dead);

        move.AddTransition(RemyStates.Idle, idle);
        move.AddTransition(RemyStates.Jump, jump);
        move.AddTransition(RemyStates.BreakDancing, breakDancing);
        move.AddTransition(RemyStates.Dead, dead);

        jump.AddTransition(RemyStates.Idle, idle);
        jump.AddTransition(RemyStates.Move, move);

        breakDancing.AddTransition(RemyStates.Idle, idle);
        breakDancing.AddTransition(RemyStates.Move, move);

        fsm.SetInitial(idle);
    }


    // Update is called once per frame
    void Update()
    {
       /* if (!IsAlive)
            return;
       */
        fsm.OnUpdate();
        /*
        // Raycast desde el centro del objeto hacia abajo
        isGrounded = Physics.Raycast(transform.position, Vector3.down, raycastDistance, groundLayer);

        if (isGrounded && !IsJumping && Input.GetKeyDown(KeyCode.Space))
        {
            IsJumping = true;
            rb.AddForce(Vector3.up * 6, ForceMode.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.Return) && energy > 0)
        {
            IsBreakDancing = !IsBreakDancing;
            energy--;
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            life = 0;
            rb.velocity = Vector3.zero;
            anim.SetTrigger("DoDie");
            return;
        }
        */
        if (Input.GetKeyDown(KeyCode.M))
        {
            life = 0;
        }

        print(fsm.GetCurrent);

        anim.SetFloat("Velocity", HorizontalVelocityMagnitude);
    }
    private void FixedUpdate()
    {
        fsm.OnFixedUpdate();
    }
    private void LateUpdate()
    {
        fsm.OnLateUpdate();
    }
    public void Jump()
    {
        IsJumping = true;
        rb.AddForce(Vector3.up * 6, ForceMode.Impulse);
    }

    public bool CanJump()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, raycastDistance, groundLayer);
        return isGrounded && !isJumping;
    }


    public void BreakDance()
    {
        IsBreakDancing = true;
        breakDancingDuration.ResetCooldown();
        energy--;
    }

    private void FinishBrakeDance()
    {
        IsBreakDancing = false;
        breakDancingCoolDown.ResetCooldown();

        print("Finished a break dance");
    }

    public bool CanBreakDance()
    {
        return energy > 0 && !breakDancingDuration.IsCooldown() && !breakDancingCoolDown.IsCooldown();
    }

    public void UpdateCooldowns()
    {
        breakDancingDuration.IsCooldown();
        breakDancingCoolDown.IsCooldown();
    }
    public void Die()
    {
        anim.SetTrigger("DoDie");
    }

    public bool IsDead()
    {
        return !IsAlive;
    }
    /*
    protected override void Move()
    {
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");

         
           // Forma cabezona para que tenga el smooth del GetAxis 
           // pero que al moverse en diagonal no se mueva mas rapido.
        
        if (h != 0 && v != 0)
        {
            h *= 0.70710678f;
            v *= 0.70710678f;
        }

        Vector3 dir = new Vector3(h, 0, v);

        dir *= speed;
        dir.y = rb.velocity.y;
        rb.velocity = dir;

        anim.SetFloat("Velocity", HorizontalVelocityMagnitude);
    }
    */

    /*
    public override void Look(Vector3 direction)
    {
        if (rb.velocity.x == 0 && rb.velocity.z == 0)
            return;

        transform.forward = direction;
    }
    */
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
