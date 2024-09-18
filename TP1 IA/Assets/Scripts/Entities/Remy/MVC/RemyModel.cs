using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemyModel : Entity, IJump, IBreakDancing, IDead
{
    [Header("Ground Check")]
    [SerializeField] LayerMask groundLayer;

    [Header("Parameters")]
    [SerializeField] float speedLimit = 6;
    [SerializeField] int energyLimit = 2;
    [SerializeField] int lifeLimit;
    [SerializeField] int breakDancingDurationTime = 3;
    [SerializeField] int breakDancingCoolDownTime = 5;
    [SerializeField] int energy;
    public int Energy { get => energy; set => energy = value; }
    
    [SerializeField] int life;
    public int Life { get => life; set => life = value; }

    readonly float raycastDistance = 0.1f;

    bool isBreakDancing;
    bool isJumping;
    bool isGrounded;
    bool isProtected;

    Cooldown breakDancingCoolDown;
    Cooldown breakDancingDuration;

    public static Action DieAction;
    public static Action JumpAction;

    protected override void Awake()
    {
        base.Awake();
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

        IsBreakDancing = false;
        IsJumping = false;

        breakDancingDuration = new(breakDancingDurationTime, FinishBrakeDance);
        breakDancingCoolDown = new(breakDancingCoolDownTime);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ChangeLife(-1);
        }
    }

    public bool IsAlive
    {
        get => life > 0;
    }

    public bool IsProtected { get => isProtected; set => isProtected = value; }

    public bool IsJumping { get => isJumping; set => isJumping = value; }

    public bool IsBreakDancing
    {
        get
        {
            return isBreakDancing;
        }
        set
        {
            isBreakDancing = value;
        }
    }
    public void BreakDance()
    {
        IsBreakDancing = true;
        breakDancingDuration.ResetCooldown();
        energy--;
    }

    public bool CanBreakDance()
    {
        return energy > 0 && !breakDancingDuration.IsCooldown() && !breakDancingCoolDown.IsCooldown();
    }
    private void FinishBrakeDance()
    {
        IsBreakDancing = false;
        breakDancingCoolDown.ResetCooldown();
    }
    public void UpdateCooldowns()
    {
        breakDancingDuration.IsCooldown();
        breakDancingCoolDown.IsCooldown();
    }
    public void Die()
    {
        DieAction?.Invoke();
        GameManager.Instance.TriggerGameOver();
    }

    public bool IsDead()
    {
        return !IsAlive;
    }

    public void Jump()
    {
        IsJumping = true;
        rb.AddForce(Vector3.up * 6, ForceMode.Impulse);
        JumpAction?.Invoke();
    }

    public bool CanJump()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, raycastDistance, groundLayer);
        return isGrounded && !isJumping;
    }

    public void ChangeLife(int amount)
    {
        life = Mathf.Clamp(life + amount, 0, lifeLimit);
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

        }
        else speed = speedLimit;
    }
}
