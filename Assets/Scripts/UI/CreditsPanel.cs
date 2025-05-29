using UnityEngine;
using UnityEngine.UI;

public class CreditosPanel : MonoBehaviour
{
    [Header("Panel de créditos")]
    public RectTransform creditosPanel;

    [Header("Panel con botones al final")]
    public GameObject botonesFinalPanel;

    [Header("Configuración")]
    public float tiempoEsperaInicio = 2f;
    public float velocidadScroll = 50f;
    public float velocidadAcelerada = 150f;

    private float velocidadActual;
    private bool estaDesplazando = false;
    private bool acelerado = false;

    private float alturaDestino;

    void Start()
    {
        if (creditosPanel == null)
        {
            Debug.LogError("CreditosPanel no asignado.");
            return;
        }

        if (botonesFinalPanel != null)
            botonesFinalPanel.SetActive(false);

        velocidadActual = velocidadScroll;
        alturaDestino = Mathf.Abs(creditosPanel.offsetMin.y);

        Invoke(nameof(EmpezarScroll), tiempoEsperaInicio);
    }

    void EmpezarScroll()
    {
        estaDesplazando = true;
    }

    void Update()
    {
        if (!estaDesplazando || creditosPanel == null)
            return;

        // Desplazar hacia arriba
        creditosPanel.anchoredPosition += Vector2.up * velocidadActual * Time.deltaTime;

        // Revisar si llegó al final
        if (creditosPanel.anchoredPosition.y >= alturaDestino)
        {
            creditosPanel.anchoredPosition = new Vector2(
                creditosPanel.anchoredPosition.x, 
                alturaDestino);

            estaDesplazando = false;
            MostrarPanelFinal();
        }
    }

    public void AcelerarCreditos()
    {
        if (!estaDesplazando) return;
        acelerado = true;
        velocidadActual = velocidadAcelerada;
    }

    public void SaltarCreditos()
    {
        estaDesplazando = false;
        creditosPanel.anchoredPosition = new Vector2(
            creditosPanel.anchoredPosition.x, 
            alturaDestino);
        MostrarPanelFinal();
    }

    void MostrarPanelFinal()
    {
        if (botonesFinalPanel != null)
            botonesFinalPanel.SetActive(true);
    }
}
