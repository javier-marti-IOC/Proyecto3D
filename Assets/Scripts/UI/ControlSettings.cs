using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlSettings : MonoBehaviour
{
    [SerializeField] private Slider cameraSensibilty;
    private SimpleThirdPersonCamera simpleThirdPersonCamera;

    void Start()
    {
        simpleThirdPersonCamera = FindObjectOfType<SimpleThirdPersonCamera>();

        if (PlayerPrefs.HasKey("cameraSensibility"))
        {
            LoadControls();
        }
        else
        {
            float defaultValue = 3f;
            cameraSensibilty.value = defaultValue;
            PlayerPrefs.SetFloat("cameraSensibility", defaultValue);
            SetCameraSensibility();
        }

        cameraSensibilty.onValueChanged.AddListener(delegate { SetCameraSensibility(); });
    }

    public void SetCameraSensibility()
    {
        int sliderValue = Mathf.RoundToInt(cameraSensibilty.value); // aseguramos número entero
        float mappedValue = 1f + sliderValue * 0.5f;

        PlayerPrefs.SetFloat("cameraSensibility", sliderValue); // guardamos valor del slider

        if (simpleThirdPersonCamera != null)
        {
            simpleThirdPersonCamera.mouseSensitivity = mappedValue;
        }
    }

    private void LoadControls()
    {
        int savedSliderValue = Mathf.RoundToInt(PlayerPrefs.GetFloat("cameraSensibility"));
        cameraSensibilty.value = savedSliderValue;
        SetCameraSensibility();
    }
}
