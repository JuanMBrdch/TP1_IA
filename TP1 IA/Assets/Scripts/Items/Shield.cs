using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Item
{    
    private void Awake()
    {
        OnCollected += ActivateShield;
    }

    private void OnDestroy()
    {
        OnCollected += ActivateShield;
    }

    private void ActivateShield(RemyController player)
    {
        player.IsProtected = true;
        Destroy(this.gameObject);
    }
}
