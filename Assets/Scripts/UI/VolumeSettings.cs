using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider environmentSlider;
    [SerializeField] private Slider uiSlider;
    [SerializeField] private Slider sfxSlider;

    public void Start()
    {
        if(PlayerPrefs.HasKey("masterVolume"))
        {
            LoadVolume();
        }
        else 
        {
            SetMasterVolume();
            SetMusicVolume();
            SetEnvironmentVolume();
            SetUIVolume();
            SetSFXVolume();
        }
        
    }
    public void SetMasterVolume()
    {
        float volume = masterSlider.value;
        audioMixer.SetFloat("Master", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("masterVolume", volume);
    }
    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        audioMixer.SetFloat("Music", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }
    public void SetEnvironmentVolume()
    {
        float volume = environmentSlider.value;
        audioMixer.SetFloat("Environment", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("environmentVolume", volume);
    }
    public void SetUIVolume()
    {
        float volume = uiSlider.value;
        audioMixer.SetFloat("UI", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("uiVolume", volume);
    }
    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        audioMixer.SetFloat("SFX", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }

    // Function to load volumes from PlayerPrefab
    private void LoadVolume()
    {
        masterSlider.value=PlayerPrefs.GetFloat("masterVolume");
        SetMasterVolume();

        musicSlider.value=PlayerPrefs.GetFloat("musicVolume");
        SetMusicVolume();

        environmentSlider.value=PlayerPrefs.GetFloat("environmentVolume");
        SetEnvironmentVolume();

        uiSlider.value=PlayerPrefs.GetFloat("uiVolume");
        SetUIVolume();

        sfxSlider.value=PlayerPrefs.GetFloat("sfxVolume");
        SetSFXVolume();
    }
}
