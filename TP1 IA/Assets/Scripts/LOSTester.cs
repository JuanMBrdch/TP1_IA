using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOSTester : MonoBehaviour
{
    public Transform target;
    LOS lineOfSight;
    // Start is called before the first frame update
    void Start()
    {
        lineOfSight = GetComponent<LOS>();
    }

    // Update is called once per frame
    void Update()
    {
        if(lineOfSight.CheckRange(target) && lineOfSight.CheckAngle(target)&& lineOfSight.InView(target))
        {
            print("DETECTED");
        }        
    }
}
