using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlienView : MonoBehaviour
{
    [SerializeField] Animator anim;
    Rigidbody rb;

    IRunAway _runningAway;
    AlienModel alienModel;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _runningAway = GetComponent<IRunAway>();
        alienModel = GetComponent<AlienModel>();

        AlienModel.AttackAction += AttackActionHandler;
    }

    private void Update()
    {
        anim.SetFloat("Velocity", new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude);
        anim.SetBool("IsRunningAway", _runningAway.IsRunningAway);
    }

    void AttackActionHandler()
    {
        anim.SetTrigger("DoAttack");
    }
    private void OnDrawGizmosSelected()
    {
        if (alienModel != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, alienModel.AttackRange);
        }
    }
}
