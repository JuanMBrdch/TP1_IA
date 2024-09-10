using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    protected Action<RemyController> OnCollected;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<RemyController>(out RemyController player))
        {
            OnCollected(player);           
        }
        
    }
}
