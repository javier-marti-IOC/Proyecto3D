using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;
using System.Diagnostics;

public class LoadingScreenManager : MonoBehaviour
{
    public RectTransform spinner; // El símbolo giratorio
    public TextMeshProUGUI loadingText;
    public float rotationSpeed = 200f;

    [SerializeField] private int sceneToLoadIndex = 2;
    [SerializeField] private float minLoadingTime = 5f; // segundos

    private void Update()
    {
        if (spinner != null)
            spinner.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime);
    }

    async void Start()
    {
        await LoadSceneAsync(sceneToLoadIndex);
    }

    private async Task LoadSceneAsync(int sceneIndex)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            if (loadingText != null)
                loadingText.text = Mathf.RoundToInt(progress * 100f) + "%";

            // Si ha terminado de cargar pero aún no activamos la escena
            if (progress >= 1f)
            {
                // Esperar hasta que se cumpla el tiempo mínimo
                float remainingTime = minLoadingTime - (float)stopwatch.Elapsed.TotalSeconds;
                if (remainingTime > 0f)
                    await Task.Delay((int)(remainingTime * 1000f));

                operation.allowSceneActivation = true;
            }

            await Task.Yield();
        }
    }
}
