using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPatrol
{
    bool patrolFinished { get; set; }
    List<Transform> Waypoints { get; }
}
