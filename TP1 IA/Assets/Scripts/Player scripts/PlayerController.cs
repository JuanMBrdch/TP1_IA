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

    }
}
