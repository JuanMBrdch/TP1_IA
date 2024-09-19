using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float speed;
    public float lifetime;
    Rigidbody rb;

    Cooldown lifetimeCooldown;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        lifetimeCooldown = new Cooldown(lifetime, Die);
        lifetimeCooldown.ResetCooldown();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.forward * speed;
        lifetimeCooldown.IsCooldown();
    }

    public Vector3 Direction
    {
        get => transform.forward;
        set => transform.forward = value;
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            RemyModel remyModel = other.GetComponent<RemyModel>();
            remyModel.ChangeLife(-1);
        }

        Die();
    }
}
