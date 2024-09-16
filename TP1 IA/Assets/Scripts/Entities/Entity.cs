using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour, IMove
{
    [Header("Parameters")]
    public float speed;
    public float rotationSpeed = 6;

    protected Rigidbody rb;

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    protected virtual void Start()
    {
        // rb = GetComponent<Rigidbody>();
    }

    public virtual void Move(Vector3 dir)
    {
        dir *= speed;
        dir.y = rb.velocity.y;
        rb.velocity = dir;
    }
    public void Look(Vector3 dir)
    {
        transform.forward = Vector3.RotateTowards(transform.forward, dir, Time.deltaTime * rotationSpeed, 0);
    }
    public void Look(Transform target)
    {
        Vector3 dir = target.position - transform.position;
        Look(dir);
    }
}
