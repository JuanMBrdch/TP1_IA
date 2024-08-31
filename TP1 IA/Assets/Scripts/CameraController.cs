using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [Header("Target")]
    public Transform target;

    [Header("Settings")]
    public Vector3 offset;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
            return;

        // this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(target.position.x, this.transform.position.y, target.position.z) + offset, speed * Time.deltaTime);
        this.transform.position = new Vector3(target.position.x, this.transform.position.y, target.position.z) + offset;
    }
}
