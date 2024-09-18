using System.Collections.Generic;
using UnityEngine;

public class CryptoStatePatrol : State<CryptoStates>
{
    Transform entity;
    List<Transform> waypoints;
    private int currentWaypoint;
    private bool reverse;
    //private bool _finished;
    //public bool finished { get { return _finished; } }
    
    IMove move;
    IPatrol patrol;
    public CryptoStatePatrol(IMove move, Transform entity, IPatrol patrol)
    {
        this.move = move;
        this.entity = entity;
        this.patrol = patrol;
        this.waypoints = patrol.Waypoints;
    }

    public override void Enter()
    {
        base.Enter();
        patrol.patrolFinished = false;
    }

    public override void Execute()
    {
        base.Execute();

        Debug.Log("patrolling");

        //****************************

        Vector3 futurePosition = waypoints[currentWaypoint].position;
        Vector3 dirToFuturePosition = (futurePosition - entity.position).normalized;
        
        move.Move(dirToFuturePosition);
 
        dirToFuturePosition.y = 0;
        move.Look(dirToFuturePosition);


        //*******
        if (Vector3.Distance(entity.position, futurePosition) <= 1.3f)
        {
            Debug.Log("Patrol point reached");
            if (currentWaypoint+1 > waypoints.Count-1)
            {
                if (reverse != true)
                {
                    reverse = true;
                    patrol.patrolFinished = true;
                }                
                //_finished = true;
            }

            if (currentWaypoint - 1 < 0)
            {
                if (reverse != false)
                {
                    reverse = false;
                    patrol.patrolFinished = true;
                }                
                //_finished = true;
            }

            if (!reverse)
            {
                currentWaypoint++;
            }
            else if (reverse) currentWaypoint--;

        }
    }
}
