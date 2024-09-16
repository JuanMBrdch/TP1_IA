using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Item
{    
    private void Start()
    {
        OnCollected += ActivateShield;
    }

    private void OnDestroy()
    {
        OnCollected += ActivateShield;
    }

    private void ActivateShield(RemyModel player)
    {
        player.IsProtected = true;
        Destroy(this.gameObject);
    }
}
