using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour, IMove
{
    [Header("Parameters")]
    public float speed;
    public float rotationSpeed = 6;

    [Header("Eye Sight")]
    [SerializeField] Transform eyeSight;

    private Rigidbody rb;

    public Transform EyeSight { get => eyeSight; set => eyeSight = value; }
    public Rigidbody Rb { get => rb; set => rb = value; }

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        Rb = GetComponent<Rigidbody>();
    }
    protected virtual void Start()
    {
        // rb = GetComponent<Rigidbody>();
    }

    public virtual void Move(Vector3 dir)
    {
        dir *= speed;
        dir.y = Rb.velocity.y;
        Rb.velocity = dir;
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
