using UnityEngine;
using TMPro;

public class FPSDisplay : MonoBehaviour
{
    public TMP_Text fpsText;  // Referencia al TextMeshPro para mostrar el texto.
    private float deltaTime = 0.0f;

    void Update()
    {
        // Calcula el tiempo entre cuadros
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        
        // Calcula los FPS
        float fps = 1.0f / deltaTime;

        // Actualiza el texto en pantalla
        if (fpsText != null)
        {
            fpsText.text = "FPS: " + Mathf.Ceil(fps).ToString();  // Muestra los FPS con un valor redondeado.
        }
    }
}
