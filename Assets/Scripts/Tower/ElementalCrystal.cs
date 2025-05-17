using UnityEngine;

public class ElementalCrystal : MonoBehaviour
{
    public float floatAmplitude = 0.2f; // Amplitud del movimiento vertical
    public float floatFrequency = 1f;   // Frecuencia del movimiento vertical

    public float swayAmplitude = 0.1f;  // Amplitud del movimiento lateral
    public float swayFrequency = 0.5f;  // Frecuencia del movimiento lateral

    public float rotationSpeed = 20f;   // Velocidad de rotación

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Movimiento vertical (levitación)
        float newY = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;

        // Movimiento lateral (hacia los lados)
        float newX = Mathf.Sin(Time.time * swayFrequency) * swayAmplitude;

        // Aplicar posición
        transform.position = startPos + new Vector3(newX, newY, 0f);

        // Rotar suavemente sobre el eje Y
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
    }
}