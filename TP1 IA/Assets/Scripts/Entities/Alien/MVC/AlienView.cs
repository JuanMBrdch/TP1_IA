using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlienView : EnemyView
{
    IRunAway _runningAway;

    override protected void Awake()
    {
        base.Awake();
        _runningAway = GetComponent<IRunAway>();
        model = GetComponent<AlienModel>();

        AlienModel.AttackAction += AttackActionHandler;
    }
    override protected void OnDestroy()
    {
        AlienModel.AttackAction -= AttackActionHandler;
    }
    override protected void Update()
    {
        base.Update();
        anim.SetBool("IsRunningAway", _runningAway.IsRunningAway);
    }
}
