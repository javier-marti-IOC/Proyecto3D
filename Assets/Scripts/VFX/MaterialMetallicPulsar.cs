using UnityEngine;

/// <summary>
/// Aplica un pulso aleatorio al valor de Metallic de un material compartido (URP Lit).
/// Todos los objetos que usen ese material mostrarán el mismo efecto.
/// </summary>
[ExecuteAlways]
public class MaterialMetallicPulsar : MonoBehaviour
{
    [Tooltip("Material al que se le aplicará el pulso en el Metallic.")]
    public Material targetMaterial;

    [Tooltip("Velocidad de oscilación del pulso.")]
    public float pulseSpeed = 1f;

    [Tooltip("Amplitud del pulso (cuánto varía el valor).")]
    [Range(0f, 1f)]
    public float pulseAmplitude = 0.2f;

    [Tooltip("Valor base de metallic alrededor del cual oscilará.")]
    [Range(0f, 1f)]
    public float baseMetallic = 0.5f;

    private float randomOffset;

    void Start()
    {
        // Offset aleatorio para que cada vez se vea distinto
        randomOffset = Random.Range(0f, 100f);
    }

    void Update()
    {
        if (targetMaterial == null) return;

        float time = Time.time + randomOffset;

        // Oscilación suave tipo pulso
        float pulse = Mathf.Sin(time * pulseSpeed) * pulseAmplitude;

        // Calculamos el nuevo valor de metallic
        float metallicValue = Mathf.Clamp01(baseMetallic + pulse);

        // Aplicamos el valor al material
        targetMaterial.SetFloat("_Metallic", metallicValue);
    }
}
