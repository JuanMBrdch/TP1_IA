using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOSTester : MonoBehaviour
{
    public Transform _target;
    LOS _lineOfSight;
    FSM<EnemyStates> _fsm;
    Cooldown _cooldown;
    Action finishCooldown = delegate { };
    
    // Start is called before the first frame update
    void Start()
    {
        _lineOfSight = GetComponent<LOS>();
        InitializeFSM();
        finishCooldown += PrintMessage;
        _cooldown = new Cooldown(10,finishCooldown);
        
    }

    private void InitializeFSM()
    {
        _fsm = new FSM<EnemyStates>();
        var idle = new EnemyStateIdle(_lineOfSight, _target, _fsm);
        var alert = new EnemyStateAlert(_lineOfSight, _target, _fsm);

        idle.AddTransition(EnemyStates.alert, alert);

        alert.AddTransition(EnemyStates.idle, idle);

        _fsm.SetInitial(idle);
    }

    // Update is called once per frame
    void Update()
    {
        //if(lineOfSight.CheckRange(target) && lineOfSight.CheckAngle(target)&& lineOfSight.InView(target))
        //{
        //    print("DETECTED");
        //}

        _fsm.OnUpdate();
        _cooldown.OnCoolDown();

    }

    void PrintMessage()
    {
        print("Cooldown Ended");
    }
}
