using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSpellObject : MonoBehaviour
{
    private GameObject target;
    private Rigidbody spellRB;
    public float fallValue;
    public float speed;
    public float maxRotationAnglePerSecond = 30f; // Limita cuanto puede girar por segundo

    void Start()
    {
        target = GameObject.FindGameObjectWithTag(Constants.player);
        spellRB = gameObject.GetComponent<Rigidbody>();
        transform.LookAt(target.transform);

        Destroy(gameObject, 1.5f); // Autodestruir después de X segundos
    }

    void FixedUpdate()
    {
        if (target == null) return;

        // Direccion deseada con caida vertical
        Vector3 direction = (new Vector3(target.transform.position.x, transform.position.y - fallValue, target.transform.position.z) - transform.position).normalized;

        // Calculamos rotacion hacia el objetivo
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Angulo maximo por frame
        float maxRotationThisFrame = maxRotationAnglePerSecond * Time.fixedDeltaTime;

        // Rotacion limitada
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, maxRotationThisFrame);

        // Movimiento hacia adelante
        spellRB.velocity = transform.forward * speed;
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        //Esto se eliminara en un futuro
        if (collision.gameObject.CompareTag(Constants.player))
        {
            Debug.Log("Proyectil agua colisiona con: " + collision.gameObject.name);
        }
        Destroy(gameObject);
    }
}
