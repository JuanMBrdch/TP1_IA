using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    //M -> Model: todo lo que el personaje hace, si dispara, salta. NO LO LLAMA ÉL
    //V -> View: representación física de los comportamientos. Un animator sería similar a esto.
    //C -> Controller: es lo que llama a los comportamientos. Cuándo saltar, cuándo hacer esto, lo otro

    private Rigidbody body;
    public float speed;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 direction) //acá  neccesitamos la dirección, nos la tiene que dar
                                            // el controller, así que la pedimos.
    {
        direction *= speed;
        direction.y = body.velocity.y;
        body.velocity = direction;
    }

    public void Look(Vector3 direction)
    {
        transform.forward = direction;
    }

    public void LookTarget(Transform target)
    {
        Vector3 direction = target.position - transform.position;
        Look(direction);
    }
}
