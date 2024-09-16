using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    protected Action<RemyModel> OnCollected;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out RemyModel player))
        {           
            OnCollected(player);            
        }
    }

    
    
}
