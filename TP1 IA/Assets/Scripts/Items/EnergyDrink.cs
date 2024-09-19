using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyDrink : Item
{
    [SerializeField] int energyToIncrease;
    private void Start()
    {
        OnCollected += IncreaseEnergy;
    }

    private void IncreaseEnergy(RemyModel player)
    {
        player.ChangeEnergy(Mathf.Abs(energyToIncrease));
        Destroy(this.gameObject);
    }
}