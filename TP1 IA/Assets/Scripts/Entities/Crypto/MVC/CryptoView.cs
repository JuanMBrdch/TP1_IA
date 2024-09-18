using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryptoView : EnemyView
{
    IClapping _clapping;

    override protected void Awake()
    {
        base.Awake();
        _clapping = GetComponent<IClapping>();
        model = GetComponent<CryptoModel>();

        CryptoModel.AttackAction += AttackActionHandler;
    }
    override protected void OnDestroy()
    {
        CryptoModel.AttackAction -= AttackActionHandler;
    }

    override protected void Update()
    {
        base.Update();
        anim.SetBool("IsClapping", _clapping.IsClapping);
    }
}
