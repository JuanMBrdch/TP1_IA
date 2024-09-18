using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPatrol
{
    List<Transform> Waypoints { get; }
    int CurrentWaypointID { get; }
    int Direction { get; }
    void WaypointReached();
}
