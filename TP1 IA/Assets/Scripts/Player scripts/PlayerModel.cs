using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    //M -> Model: todo lo que el personaje hace, si dispara, salta. NO LO LLAMA �L
    //V -> View: representaci�n f�sica de los comportamientos. Un animator ser�a similar a esto.
    //C -> Controller: es lo que llama a los comportamientos. Cu�ndo saltar, cu�ndo hacer esto, lo otro

    private Rigidbody body;
    public float speed;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 direction) //ac�  neccesitamos la direcci�n, nos la tiene que dar
                                            // el controller, as� que la pedimos.
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
