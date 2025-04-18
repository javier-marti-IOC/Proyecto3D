using UnityEngine;

/// <summary>
/// Aplica un destello muy corto al valor de Metallic de un material URP Lit.
/// El valor vuelve inmediatamente a su estado original después del flash.
/// </summary>
[ExecuteAlways]
public class MaterialMetallicFlash : MonoBehaviour
{
    [Tooltip("Material compartido URP Lit al que se le aplica el destello.")]
    public Material targetMaterial;

    [Tooltip("Valor base de metallic que tendrá normalmente.")]
    [Range(0f, 1f)]
    public float baseMetallic = 0.2f;

    [Tooltip("Valor de metallic durante el destello.")]
    [Range(0f, 1f)]
    public float flashValue = 1f;

    [Tooltip("Tiempo mínimo entre destellos.")]
    public float minInterval = 0.5f;

    [Tooltip("Tiempo máximo entre destellos.")]
    public float maxInterval = 2f;

    [Tooltip("Duración del destello (segundos).")]
    public float flashDuration = 0.05f;

    private float nextFlashTime;
    private float flashEndTime;
    private bool isFlashing;

    void Start()
    {
        ScheduleNextFlash();
    }

    void Update()
    {
        if (targetMaterial == null) return;

        float currentTime = Time.time;

        if (isFlashing)
        {
            if (currentTime >= flashEndTime)
            {
                // Fin del destello, volver al valor base
                targetMaterial.SetFloat("_Metallic", baseMetallic);
                isFlashing = false;
                ScheduleNextFlash();
            }
        }
        else
        {
            if (currentTime >= nextFlashTime)
            {
                // Inicia el destello
                targetMaterial.SetFloat("_Metallic", flashValue);
                isFlashing = true;
                flashEndTime = currentTime + flashDuration;
            }
        }
    }

    void ScheduleNextFlash()
    {
        nextFlashTime = Time.time + Random.Range(minInterval, maxInterval);
    }
}
