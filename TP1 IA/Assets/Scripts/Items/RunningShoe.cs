using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningShoe : Item
{    
    [SerializeField] float speedIncrease;
    void Start()
    {
        OnCollected += IncreaseSpeed;
    }

    private void IncreaseSpeed(RemyController player)
    {
        player.ChangeSpeed(Mathf.Abs(speedIncrease));
        Destroy(this.gameObject);
    }

}
