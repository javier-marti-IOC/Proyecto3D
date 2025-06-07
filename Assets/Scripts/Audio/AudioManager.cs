using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Mixer")]
    public AudioMixer mixer;

    [Header("Sonidos")]
    public Sound[] music;
    public Sound[] sfx;
    public Sound[] ui;
    public Sound[] ambience;
    public Sound[] dialogue;

    private Dictionary<string, Sound> soundDictionary = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitCategory(music);
            InitCategory(sfx);
            InitCategory(ui);
            InitCategory(ambience);
            InitCategory(dialogue);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitCategory(Sound[] sounds)
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = s.playOnAwake;
            s.source.spatialize = s.spatialize;
            s.source.spatialBlend = s.spatialBlend;
            s.source.outputAudioMixerGroup = s.outputGroup;

            if (s.playOnAwake)
                s.source.Play();

            if (!soundDictionary.ContainsKey(s.name))
                soundDictionary.Add(s.name, s);
        }
    }

    public void Play(string name)
    {
        if (soundDictionary.TryGetValue(name, out var s))
            s.source.Play();
        else
            Debug.LogWarning("No se encontró el sonido: " + name);
    }

    public void Stop(string name)
    {
        if (soundDictionary.TryGetValue(name, out var s))
            s.source.Stop();
    }

    public void SetIndividualVolume(string name, float volume)
    {
        if (soundDictionary.TryGetValue(name, out var s))
        {
            s.volume = volume;
            s.source.volume = volume;
        }
    }

    public void SetCategoryVolume(string parameterName, float linearVolume)
    {
        float volumeDb = Mathf.Log10(Mathf.Clamp(linearVolume, 0.0001f, 1f)) * 20f;
        mixer.SetFloat(parameterName, volumeDb);
    }

    public float GetCategoryVolume(string parameterName)
    {
        if (mixer.GetFloat(parameterName, out float dbVolume))
            return Mathf.Pow(10f, dbVolume / 20f);
        return 1f;
    }

    public AudioSource GetAudioSourceByName(string name)
    {
        if (soundDictionary.TryGetValue(name, out var s))
            return s.source;

        Debug.LogWarning("No se encontró el AudioSource para: " + name);
        return null;
    }
}
