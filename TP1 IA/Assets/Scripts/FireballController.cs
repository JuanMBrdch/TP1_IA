using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class FireballController : MonoBehaviour
{
    public float speed;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.forward * speed;
    }

    public Vector3 Direction
    {
        get => transform.forward;
        set => transform.forward = value;
    }

    private void OnTriggerEnter(Collider other)
    {
        // TODO: DO Damage to player
        Destroy(this.gameObject);
    }
}
