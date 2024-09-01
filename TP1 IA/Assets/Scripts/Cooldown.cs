using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooldown
{
    private float _cooldownTime;
    private float _currentTime;
    Action OnFinishedCooldown;
    public Cooldown(float time, Action finished = null, bool startOnCooldown = false)
    {
        _cooldownTime = time;
        OnFinishedCooldown = finished;
        if (startOnCooldown)
        {
            Reset();
        }
        else
        {
            _currentTime = 0;
        }
    }

    public bool OnCoolDown()
    {
        if (_currentTime > 0)
        {
           _currentTime -= Time.deltaTime;
            return true;
        }        
        
        if (_currentTime <= 0)
        {
            OnFinishedCooldown();
            Reset();
            return false;
        }

        return true;
    }

    public void Reset()
    {
        _currentTime = _cooldownTime;
    }
}
