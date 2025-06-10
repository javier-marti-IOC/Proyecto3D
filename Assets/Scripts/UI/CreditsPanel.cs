using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CreditosPanel : MonoBehaviour
{
    [Header("Panel de créditos")]
    public RectTransform creditosPanel;

    [Header("Panel con botones al final")]
    public GameObject botonesFinalPanel;

    [Header("Configuración")]
    public float tiempoEsperaInicio = 3f;
    public float velocidadScroll = 75f;
    public float velocidadAcelerada = 400f;
    public float suavizadoAceleracion = 3f;
    public Button button;
    [Header("Input Action para saltar créditos")]
    public InputActionReference pauseAction;

    private float alturaMostrarBotones;
    private float velocidadActual;
    private bool estaDesplazando = false;
    private float alturaDestino;
    private bool panelMostrado = false;

    void Start()
    {
        if (creditosPanel == null)
        {
            Debug.LogError("Debe asignar el Creditospanel");
            return;
        }

        if (botonesFinalPanel != null)
            botonesFinalPanel.SetActive(false);

        // Calcular alturaMostrarBotones para que aparezca cuando el panel haya salido completamente
        alturaMostrarBotones = 5600;

        velocidadActual = velocidadScroll;

        // alturaDestino la dejamos igual (para evitar pasar el límite)
        alturaDestino = Mathf.Abs(creditosPanel.offsetMin.y);

        Invoke(nameof(EmpezarScroll), tiempoEsperaInicio);
    }

    private void OnEnable()
    {
        if (pauseAction != null)
            pauseAction.action.performed += OnPause;
    }

    private void OnDisable()
    {
        if (pauseAction != null)
            pauseAction.action.performed -= OnPause;
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        SaltarCreditos(); // <-- Al pulsar el botón Pause, se salta directamente
    }

    void EmpezarScroll()
    {
        estaDesplazando = true;
    }

    void Update()
    {
        if (!estaDesplazando || creditosPanel == null)
            return;

        float velocidadObjetivo = Input.anyKey ? velocidadAcelerada : velocidadScroll;
        velocidadActual = Mathf.Lerp(velocidadActual, velocidadObjetivo, Time.deltaTime * suavizadoAceleracion);

        creditosPanel.anchoredPosition += Vector2.up * velocidadActual * Time.deltaTime;

        if (!panelMostrado && creditosPanel.anchoredPosition.y >= alturaMostrarBotones)
        {
            MostrarPanelFinal();
        }

        if (creditosPanel.anchoredPosition.y >= alturaDestino)
        {
            creditosPanel.anchoredPosition = new Vector2(
                creditosPanel.anchoredPosition.x,
                alturaDestino);

            estaDesplazando = false;
        }
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
        if (button != null)
        {
            button.Select();
        }

        panelMostrado = true;
    }
    
    // public void backToMenu() // Botón "Volver al menú principal"
    // {
    //     SceneManager.LoadScene(0);
    //     Debug.Log("Sortir al menú");
    // }
}
