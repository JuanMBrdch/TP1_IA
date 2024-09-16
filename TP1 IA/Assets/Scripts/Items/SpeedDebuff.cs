using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedDebuff : Item
{
    [SerializeField] float speedDecrease;
    void Start()
    {
        OnCollected += DecreaseSpeed;
    }

    private void DecreaseSpeed(RemyModel player)
    {
        player.ChangeSpeed(Mathf.Abs(speedDecrease)*-1);
        Destroy(this.gameObject);
    }
}
