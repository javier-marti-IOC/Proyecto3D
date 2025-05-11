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

        // Elevar un poco el punto de mira para que apunte "ligeramente por encima" del objetivo
        Vector3 targetPosition = target.transform.position + new Vector3(0f, 1f, 0f); // Eleva 1 unidad en Y
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Calculamos rotación hacia el objetivo elevado
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Ángulo máximo por frame
        float maxRotationThisFrame = maxRotationAnglePerSecond * Time.fixedDeltaTime;

        // Rotación limitada
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
