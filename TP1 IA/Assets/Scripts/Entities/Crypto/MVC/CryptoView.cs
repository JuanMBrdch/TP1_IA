using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class CryptoView : MonoBehaviour
{
    [SerializeField] Animator anim;
    Rigidbody rb;

    IClapping _clapping;
    CryptoModel cryptoModel;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _clapping = GetComponent<IClapping>();
        cryptoModel = GetComponent<CryptoModel>();

        CryptoModel.AttackAction += AttackActionHandler;
    }
    private void OnDestroy()
    {
        CryptoModel.AttackAction -= AttackActionHandler;

    }
        // Update is called once per frame
        void Update()
    {
        anim.SetFloat("Velocity", new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude);
        anim.SetBool("IsClapping", _clapping.IsClapping);
    }

    void AttackActionHandler()
    {
        anim.SetTrigger("DoAttack");
    }

    private void OnDrawGizmosSelected()
    {
        if (cryptoModel != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, cryptoModel.AttackRange);
        }
    }
}
