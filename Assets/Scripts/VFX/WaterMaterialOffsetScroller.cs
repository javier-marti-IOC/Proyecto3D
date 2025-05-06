using UnityEngine;

[ExecuteAlways]
public class WaterMaterialSmoothFlow : MonoBehaviour
{
    [Tooltip("Material que comparte todos los objetos con agua.")]
    public Material targetMaterial;

    [Tooltip("Velocidad de desplazamiento.")]
    public float movementSpeed = 0.05f;

    [Tooltip("Ángulo base de dirección del flujo en grados (0 = derecha, 90 = arriba, 180 = izquierda, 270 = abajo).")]
    [Range(0, 360)]
    public float baseDirectionAngle = 270f;

    [Tooltip("Intensidad del desvío generado por el Perlin Noise (en grados).")]
    public float deviationAngle = 15f;

    [Tooltip("Escala del ruido Perlin para la dirección.")]
    public float noiseScale = 0.2f;

    private Vector2 offset;

    void Update()
    {
        if (targetMaterial == null) return;

        float time = Time.time;

        // Dirección suavemente variada con Perlin
        float noiseValue = Mathf.PerlinNoise(time * noiseScale, 0f);
        float angleInRadians = (baseDirectionAngle + (noiseValue - 0.5f) * 2f * deviationAngle) * Mathf.Deg2Rad;

        Vector2 direction = new Vector2(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians));
        offset += direction * movementSpeed * Time.deltaTime;

        // Limita el offset al rango [0,1] para evitar acumulación infinita
        offset.x %= 1f;
        offset.y %= 1f;

        targetMaterial.mainTextureOffset = offset;
    }
}
