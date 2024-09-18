using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyView : MonoBehaviour
{
    [SerializeField] protected Animator anim;
    protected Rigidbody rb;

    protected EnemyModel model;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        model = GetComponent<EnemyModel>();
    }
    protected abstract void OnDestroy();

    protected virtual void Update()
    {
        anim.SetFloat("Velocity", new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude);
    }

    protected void AttackActionHandler()
    {
        anim.SetTrigger("DoAttack");
    }

    private void OnDrawGizmosSelected()
    {
        if (model != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, model.AttackRange);
        }
    }
}
