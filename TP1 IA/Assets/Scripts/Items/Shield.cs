using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Item
{    private void Start()
    {
        OnCollected += ActivateShield;
    }

    private void ActivateShield(RemyController player)
    {
        player.isProtected = true;
        Destroy(this.gameObject);
    }
}
