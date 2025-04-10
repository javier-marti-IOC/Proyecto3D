using UnityEngine;

public class EmissivePulseShared : MonoBehaviour
{
    [Header("Configuración del Emissivo")]
    public Material targetMaterial;
    public Color emissiveColor = Color.white;
    public float minIntensity = 0f;
    public float maxIntensity = 5f;
    public float speed = 2f;

    private float currentTime = 0f;

    void Start()
    {
        if (targetMaterial != null)
        {
            targetMaterial.EnableKeyword("_EMISSION");
        }
    }

    void Update()
    {
        if (targetMaterial == null) return;

        float pulse = Mathf.Lerp(minIntensity, maxIntensity, (Mathf.Sin(currentTime * speed) + 1f) / 2f);
        targetMaterial.SetColor("_EmissionColor", emissiveColor * pulse);
        currentTime += Time.deltaTime;
    }
}
