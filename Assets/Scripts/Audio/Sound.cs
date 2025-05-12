using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)] public float volume = 1f;
    [Range(-3f, 3f)] public float pitch = 1f;

    public bool loop = false;                 // ¿Se repite continuamente?
    public bool playOnAwake = false;          // ¿Se reproduce al iniciar?
    public bool spatialize = false;           // Sonido 3D
    [Range(0f, 1f)] public float spatialBlend = 0f; // 0 = 2D, 1 = 3D

    public AudioMixerGroup outputGroup;

    [HideInInspector] public AudioSource source;
}