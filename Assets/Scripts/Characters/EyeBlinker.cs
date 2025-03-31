using UnityEngine;
using System.Collections;

public class EyeBlinker : MonoBehaviour
{
    [Header("Materiales")]
    public Material eyesOpenMaterial;
    public Material eyesClosedMaterial;

    [Header("Configuración")]
    public float minTimeBetweenBlinks = 2f;
    public float maxTimeBetweenBlinks = 5f;
    public float blinkDuration = 0.15f; // ⏱️ Duración del parpadeo

    private SkinnedMeshRenderer eyeRenderer;
    private float blinkTimer;

    void Start()
    {
        eyeRenderer = GetComponent<SkinnedMeshRenderer>();
        ScheduleNextBlink();
    }

    void Update()
    {
        blinkTimer -= Time.deltaTime;

        if (blinkTimer <= 0)
        {
            StartCoroutine(Blink());
            ScheduleNextBlink();
        }
    }

    void ScheduleNextBlink()
    {
        blinkTimer = Random.Range(minTimeBetweenBlinks, maxTimeBetweenBlinks);
    }

    IEnumerator Blink()
    {
        // Cierra los ojos
        eyeRenderer.material = eyesClosedMaterial;
        yield return new WaitForSeconds(blinkDuration); // Espera visiblemente
        // Abre los ojos
        eyeRenderer.material = eyesOpenMaterial;
    }
}
