using System.Collections.Generic;
using UnityEngine;

public class CryptoStatePatrol : State<CryptoStates>
{
    Transform entity;
    List<Transform> waypoints;
    
    IMove move;
    IPatrol patrol;
    public CryptoStatePatrol(IMove move, Transform entity, IPatrol patrol)
    {
        this.move = move;
        this.entity = entity;
        this.patrol = patrol;
        this.waypoints = patrol.Waypoints;
    }

    public override void Execute()
    {
        base.Execute();

        //TODO: Implement patrolling.
    }
}
