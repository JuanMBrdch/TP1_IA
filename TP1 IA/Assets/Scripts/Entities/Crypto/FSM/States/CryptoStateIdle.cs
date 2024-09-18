using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryptoStateIdle : State<CryptoStates>
{
    IMove move;
    IIdle idle;
    private float _idleTime;
    private float _currentIdleTime;
    public CryptoStateIdle(IMove move, IIdle idle ,float duration)
    {
        this.move = move;
        this.idle = idle;
        this._idleTime = duration;
    }

    public override void Enter()
    {
        base.Enter();
        _currentIdleTime = 0;
        idle.IdleFinished = false;
        move.Move(Vector3.zero);
    }

    public override void Execute()
    {
        base.Execute();
        if(_currentIdleTime < _idleTime)
        {
            _currentIdleTime += Time.deltaTime;
        }
        else
        {
            idle.IdleFinished = true;
        }
        Debug.Log("Idle");
    }
}
