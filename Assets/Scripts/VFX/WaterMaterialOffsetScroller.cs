using UnityEngine;

/// <summary>
/// Mueve el offset del material con una dirección que cambia suavemente gracias a Perlin Noise.
/// </summary>
[ExecuteAlways]
public class WaterMaterialSmoothFlow : MonoBehaviour
{
    [Tooltip("Material que comparte todos los objetos con agua.")]
    public Material targetMaterial;

    [Tooltip("Velocidad de desplazamiento.")]
    public float movementSpeed = 0.05f;

    [Tooltip("Escala del ruido Perlin para la dirección.")]
    public float noiseScale = 0.2f;

    private Vector2 offset;

    void Update()
    {
        if (targetMaterial == null) return;

        float time = Time.time;

        // Usamos Perlin para generar una dirección suave
        float angle = Mathf.PerlinNoise(time * noiseScale, 0f) * Mathf.PI * 2f;

        // Convertimos ese ángulo en una dirección
        Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

        // Avanzamos el offset de forma continua en esa dirección
        offset += direction * movementSpeed * Time.deltaTime;

        // Aplicamos el offset al material
        targetMaterial.mainTextureOffset = offset;
    }
}
