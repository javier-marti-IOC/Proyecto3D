using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class MusicTowerZoneTrigger : MonoBehaviour
{
    [Header("Collider principal del jugador")]
    public Collider playerCollider;  // SphereCollider player
    private bool isPlayerInside = false;

    [Header("Fade Settings")]
    public float fadeDuration = 8f;
    public float maxVolume = 0f;

    private Coroutine fadeCoroutine;
    private AudioSource towerMusicSource;

    private void Start()
    {
        // Asume que AudioManager tiene un método GetAudioSourceByName
        towerMusicSource = AudioManager.Instance.GetAudioSourceByName("musicTower");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Time.deltaTime == 0f) return;
        if (other == playerCollider && !isPlayerInside)
        {
            isPlayerInside = true;
            PlayMusic();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (Time.deltaTime == 0f) return;
        if (other == playerCollider && isPlayerInside)
        {
            isPlayerInside = false;
            StopMusic();
        }
    }

    public void PlayMusic()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeIn(towerMusicSource, fadeDuration));
    }

    public void StopMusic()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeOut(towerMusicSource, fadeDuration));
    }

    private IEnumerator FadeIn(AudioSource source, float duration)
    {
        source.volume = 0f;
        source.Play();

        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            source.volume = Mathf.Lerp(0f, maxVolume, time / duration);
            yield return null;
        }

        source.volume = maxVolume;
    }

    private IEnumerator FadeOut(AudioSource source, float duration)
    {
        float startVolume = source.volume;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, 0f, time / duration);
            yield return null;
        }

        source.volume = 0f;
        source.Stop();
    }
}
