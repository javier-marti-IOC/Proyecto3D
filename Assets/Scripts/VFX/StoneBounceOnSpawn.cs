using UnityEngine;

public class StoneBounceOnSpawn : MonoBehaviour
{
    public float upwardForceMin = 2f;
    public float upwardForceMax = 4f;
    public float lateralForceRange = 1f;
    public float torqueForce = 5f;

    private Rigidbody rb;

    void OnEnable()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();

        // Asegúrate de que la piedra esté sin velocidad previa
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Calcular fuerza aleatoria
        float upwardForce = Random.Range(upwardForceMin, upwardForceMax);
        float xForce = Random.Range(-lateralForceRange, lateralForceRange);
        float zForce = Random.Range(-lateralForceRange, lateralForceRange);

        Vector3 launchForce = new Vector3(xForce, upwardForce, zForce);
        rb.AddForce(launchForce, ForceMode.Impulse);

        // Añadir torque (rotación aleatoria)
        Vector3 randomTorque = new Vector3(
            Random.Range(-torqueForce, torqueForce),
            0f, // rotación Y no necesaria si solo quieres X y Z
            Random.Range(-torqueForce, torqueForce)
        );

        rb.AddTorque(randomTorque, ForceMode.Impulse);
    }
}
