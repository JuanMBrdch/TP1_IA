using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateIdle : State<EnemyStates>
{
    FSM<EnemyStates> _fsm;
    LOS _lineOfSight;
    Transform _target;
   public EnemyStateIdle(LOS los, Transform target, FSM<EnemyStates> fsm)
    {
        _lineOfSight = los;
        _target = target;
        _fsm = fsm;
    }

    public override void Execute()
    {
        base.Execute();
        if (_lineOfSight.CheckRange(_target) && _lineOfSight.CheckAngle(_target) && _lineOfSight.InView(_target))
        {
            _fsm.Transition(EnemyStates.alert);
        }
    }
}
