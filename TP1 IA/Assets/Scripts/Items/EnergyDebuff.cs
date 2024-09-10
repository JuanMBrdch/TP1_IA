using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyDebuff : Item
{
    [SerializeField] int energyToDecrease;
    private void Start()
    {
        OnCollected += DecreaseEnergy;
    }

    private void DecreaseEnergy(RemyController player)
    {
        player.ChangeEnergy(Mathf.Abs(energyToDecrease) * -1);
        Destroy(this.gameObject);
    }
}
