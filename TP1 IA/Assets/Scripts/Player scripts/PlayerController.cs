using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    IMove playerModel;

    private void Start()
    {
        playerModel = GetComponent<IMove>();
    }

    private void Update()
    {
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");
        
        Vector3 dir = new Vector3 (h, 0, v);
        
        playerModel.Move(dir.normalized);

        if (h  != 0 || v != 0)
        {
            playerModel.Look(dir.normalized);
        } 
    }
}
