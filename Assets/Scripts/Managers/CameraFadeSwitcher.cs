using UnityEngine;
using UnityEngine.UI;

public class CameraFadeSwitcher : MonoBehaviour
{
    public Image fadeImage;
    public Camera cameraFrom;
    public Camera cameraTo;
    public GameObject compassBar; // Compass Bar que se desactiva temporalmente
    public float fadeDuration = 1f;
    public float waitDuration = 5f;

    private enum FadeState
    {
        Idle,
        FadingOutToSecond,
        FadingInToSecond,
        WaitingOnSecond,
        FadingOutToFirst,
        FadingInToFirst
    }

    private FadeState currentState = FadeState.Idle;

    private float fadeTimer = 0f;
    private float waitTimer = 0f;
    private float startAlpha = 0f;
    private float endAlpha = 0f;

    private bool wasCompassBarActive = false;

    void Update()
    {
        switch (currentState)
        {
            case FadeState.FadingOutToSecond:
                HandleFade(FadeState.FadingInToSecond, () =>
                {
                    // Guardar y desactivar Compass Bar
                    if (compassBar != null)
                    {
                        wasCompassBarActive = compassBar.activeSelf;
                        compassBar.SetActive(false);
                    }
                    SwitchCameras(cameraFrom, cameraTo);
                });
                break;

            case FadeState.FadingInToSecond:
                HandleFade(FadeState.WaitingOnSecond);
                break;

            case FadeState.WaitingOnSecond:
                waitTimer += Time.deltaTime;
                if (waitTimer >= waitDuration)
                {
                    StartFade(0f, 1f, FadeState.FadingOutToFirst);
                }
                break;

            case FadeState.FadingOutToFirst:
                HandleFade(FadeState.FadingInToFirst, () =>
                {
                    SwitchCameras(cameraTo, cameraFrom);
                    // Restaurar Compass Bar si estaba activo antes
                    if (compassBar != null && wasCompassBarActive)
                    {
                        compassBar.SetActive(true);
                    }
                });
                break;

            case FadeState.FadingInToFirst:
                HandleFade(FadeState.Idle);
                break;
        }
    }

    public void SwitchCameraWithFade()
    {
        if (currentState == FadeState.Idle)
        {
            fadeImage.gameObject.SetActive(true);
            StartFade(0f, 1f, FadeState.FadingOutToSecond);
        }
    }

    private void StartFade(float from, float to, FadeState nextState)
    {
        startAlpha = from;
        endAlpha = to;
        fadeTimer = 0f;
        currentState = nextState;
    }

    private void HandleFade(FadeState nextState, System.Action onFadeComplete = null)
    {
        fadeTimer += Time.deltaTime;
        SetFadeAlpha(Mathf.Lerp(startAlpha, endAlpha, fadeTimer / fadeDuration));
        if (fadeTimer >= fadeDuration)
        {
            SetFadeAlpha(endAlpha);
            onFadeComplete?.Invoke();

            if (nextState == FadeState.WaitingOnSecond)
                waitTimer = 0f;

            StartFade(endAlpha, endAlpha == 1f ? 0f : 1f, nextState);
        }
    }

    private void SetFadeAlpha(float alpha)
    {
        Color c = fadeImage.color;
        c.a = alpha;
        fadeImage.color = c;
    }

    private void SwitchCameras(Camera camOff, Camera camOn)
    {
        camOff.gameObject.SetActive(false);
        camOn.gameObject.SetActive(true);
    }
}
