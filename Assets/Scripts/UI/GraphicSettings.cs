using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GraphicSettings : MonoBehaviour
{
    public TMP_Dropdown qualityDropdown;
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown screenModeDropdown;
    public Toggle vsyncToggle;
    public TMP_Dropdown shadowsDropdown;
    public TMP_Dropdown drawDistanceDropdown;
    public TMP_Dropdown fpsLimitDropdown;


    private Resolution[] resolutions;
    private List<Resolution> uniqueResolutionsList = new List<Resolution>(); // Lista de resoluciones únicas

    void Start()
    {
        PopulateResolutionDropdown();
        //PopulateQualityDropdown();
        PopulateScreenModeDropdown();
        // PopulateShadowsDropdown();
        // PopulateDrawDistanceDropdown();
        PopulateFPSLimitDropdown();
        LoadSettings();
    }

    void PopulateResolutionDropdown()
    {
        if (resolutionDropdown == null)
        {
            Debug.LogError("resolutionDropdown no está asignado en el Inspector.");
            return;
        }

        // Lista común de resoluciones 16:9 populares
        List<Vector2Int> desiredResolutions = new List<Vector2Int>
        {
            new Vector2Int(1280, 720),
            new Vector2Int(1366, 768),
            new Vector2Int(1600, 900),
            new Vector2Int(1920, 1080),
            new Vector2Int(2560, 1440),
            new Vector2Int(3200, 1800),
            new Vector2Int(3840, 2160)
        };

        // Obtener resoluciones disponibles
        resolutions = Screen.resolutions;
        HashSet<string> usedRes = new HashSet<string>();
        List<string> options = new List<string>();
        uniqueResolutionsList.Clear();

        int currentResolutionIndex = 0;

        foreach (var res in desiredResolutions)
        {
            Resolution match = System.Array.Find(resolutions, r => r.width == res.x && r.height == res.y);
            if (match.width != 0 && match.height != 0)
            {
                string resString = $"{match.width}x{match.height}";
                if (!usedRes.Contains(resString))
                {
                    usedRes.Add(resString);
                    options.Add(resString);
                    uniqueResolutionsList.Add(match);

                    if (match.width == Screen.currentResolution.width && match.height == Screen.currentResolution.height)
                        currentResolutionIndex = options.Count - 1;
                }
            }
        }

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.onValueChanged.RemoveAllListeners();
        resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionIndex", currentResolutionIndex);
        resolutionDropdown.RefreshShownValue();
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }
    // void PopulateResolutionDropdown()
    // {
    //     if (resolutionDropdown == null)
    //     {
    //         Debug.LogError("resolutionDropdown no está asignado en el Inspector.");
    //         return;
    //     }

    //     resolutions = Screen.resolutions;
    //     resolutionDropdown.ClearOptions();
        
    //     if (resolutions.Length == 0)
    //     {
    //         Debug.LogError("No se encontraron resoluciones disponibles.");
    //         return;
    //     }

    //     // Usamos un HashSet para almacenar resoluciones únicas
    //     HashSet<string> uniqueResolutions = new HashSet<string>();
    //     List<string> options = new List<string>();
    //     uniqueResolutionsList.Clear(); // Limpiamos la lista antes de llenarla con resoluciones únicas

    //     int currentResolutionIndex = 0;

    //     for (int i = 0; i < resolutions.Length; i++)
    //     {
    //         string option = resolutions[i].width + "x" + resolutions[i].height;

    //         // Solo agregamos resoluciones únicas
    //         if (!uniqueResolutions.Contains(option))
    //         {
    //             uniqueResolutions.Add(option);
    //             options.Add(option);
    //             uniqueResolutionsList.Add(resolutions[i]); // Añadimos la resolución a la lista de resoluciones únicas
    //         }

    //         if (resolutions[i].width == Screen.currentResolution.width &&
    //             resolutions[i].height == Screen.currentResolution.height)
    //         {
    //             // Ajustamos el índice de la resolución actual en la lista filtrada
    //             currentResolutionIndex = options.Count - 1;
    //         }
    //     }

    //     resolutionDropdown.AddOptions(options);
    //     resolutionDropdown.onValueChanged.RemoveAllListeners();  // 🚨 Evita llamadas prematuras
    //     resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionIndex", currentResolutionIndex);
    //     resolutionDropdown.RefreshShownValue();
    //     resolutionDropdown.onValueChanged.AddListener(SetResolution);
    // }

    // void PopulateQualityDropdown()
    // {
    //     if (qualityDropdown == null)
    //     {
    //         Debug.LogError("qualityDropdown no está asignado en el Inspector.");
    //         return;
    //     }

    //     // Obtener las opciones de calidad desde los nombres de los niveles de calidad.
    //     List<string> options = new List<string>(QualitySettings.names);

    //     qualityDropdown.ClearOptions();
    //     qualityDropdown.AddOptions(options);
    //     qualityDropdown.onValueChanged.RemoveAllListeners();
    //     qualityDropdown.value = PlayerPrefs.GetInt("Quality", 2);
    //     qualityDropdown.RefreshShownValue();
    //     qualityDropdown.onValueChanged.AddListener(SetQuality);
    // }
    void PopulateScreenModeDropdown()
    {
        if (screenModeDropdown == null)
        {
            Debug.LogError("screenModeDropdown no está asignado en el Inspector.");
            return;
        }

        // Modo de pantalla disponibles
        List<string> options = new List<string>
        {
            "Ventana",                 // FullScreenMode.Windowed
            "Pantalla Completa",       // FullScreenMode.ExclusiveFullScreen
            "Pantalla Completa (Sin bordes)"  // FullScreenMode.FullScreenWindow
        };

        screenModeDropdown.ClearOptions();
        screenModeDropdown.AddOptions(options);
        screenModeDropdown.onValueChanged.RemoveAllListeners();
        screenModeDropdown.value = PlayerPrefs.GetInt("ScreenMode", 0);
        screenModeDropdown.RefreshShownValue();
        screenModeDropdown.onValueChanged.AddListener(SetScreenMode);
    }
    // void PopulateShadowsDropdown()
    // {
    //     if (shadowsDropdown == null)
    //     {
    //         Debug.LogError("shadowsDropdown no está asignado en el Inspector.");
    //         return;
    //     }

    //     // Obtener las opciones de sombras desde ShadowQuality
    //     List<string> options = new List<string>
    //     {
    //         "Ninguna", // ShadowQuality.Disable
    //         "Baja",    // ShadowQuality.HardOnly
    //         "Media",   // ShadowQuality.All
    //         "Alta"     // ShadowQuality.All
    //     };

    //     shadowsDropdown.ClearOptions();
    //     shadowsDropdown.AddOptions(options);
    //     shadowsDropdown.onValueChanged.RemoveAllListeners();
    //     shadowsDropdown.value = PlayerPrefs.GetInt("Shadows", 2);
    //     shadowsDropdown.RefreshShownValue();
    //     shadowsDropdown.onValueChanged.AddListener(SetShadows);
    // }
    // void PopulateDrawDistanceDropdown()
    // {
    //     if (drawDistanceDropdown == null)
    //     {
    //         Debug.LogError("drawDistanceDropdown no está asignado en el Inspector.");
    //         return;
    //     }

    //     // Opciones predefinidas para la distancia de dibujo
    //     List<string> options = new List<string>
    //     {
    //         "300m", 
    //         "500m", 
    //         "750m",
    //         "1000m",
    //         "2000m"
    //     };

    //     drawDistanceDropdown.ClearOptions();
    //     drawDistanceDropdown.AddOptions(options);
    //     drawDistanceDropdown.onValueChanged.RemoveAllListeners();
    //     drawDistanceDropdown.value = PlayerPrefs.GetInt("DrawDistance", 1);
    //     drawDistanceDropdown.RefreshShownValue();
    //     drawDistanceDropdown.onValueChanged.AddListener(SetDrawDistance);
    // }
    void PopulateFPSLimitDropdown()
    {
        if (fpsLimitDropdown == null)
        {
            Debug.LogError("fpsLimitDropdown no está asignado en el Inspector.");
            return;
        }

        // Opciones predefinidas para el límite de FPS
        List<string> options = new List<string>
        {
            "30 FPS", 
            "60 FPS", 
            "120 FPS", 
            "144 FPS",
            "165 FPS",
            "Sin Límite"
        };

        fpsLimitDropdown.ClearOptions();
        fpsLimitDropdown.AddOptions(options);
        fpsLimitDropdown.onValueChanged.RemoveAllListeners();
        fpsLimitDropdown.value = PlayerPrefs.GetInt("FPSLimit", 2);
        fpsLimitDropdown.RefreshShownValue();
        fpsLimitDropdown.onValueChanged.AddListener(SetFPSLimit);
    }

    // public void SetQuality(int qualityIndex)
    // {
    //     QualitySettings.SetQualityLevel(qualityIndex);
    //     PlayerPrefs.SetInt("Quality", qualityIndex);
    //     PlayerPrefs.Save();
    // }

    public void SetResolution(int resolutionIndex)
    {
        if (uniqueResolutionsList == null || uniqueResolutionsList.Count == 0 || resolutionIndex < 0 || resolutionIndex >= uniqueResolutionsList.Count)
        {
            Debug.LogWarning("Índice de resolución inválido.");
            return;
        }

        Resolution resolution = uniqueResolutionsList[resolutionIndex]; // Usamos el índice en la lista filtrada
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode);
        PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex);
        PlayerPrefs.Save();
    }

    public void SetScreenMode(int modeIndex)
    {
        FullScreenMode mode = (FullScreenMode)modeIndex;

        // Dependiendo del valor de modeIndex, asignar el modo de pantalla adecuado
        if (modeIndex == 0)
        {
            // Modo ventana (sin bordes)
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
        else if (modeIndex == 1)
        {
            // Modo pantalla completa tradicional
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        else if (modeIndex == 2)
        {
            // Modo pantalla completa sin bordes
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }

        // Guarda la opción seleccionada en PlayerPrefs
        PlayerPrefs.SetInt("ScreenMode", modeIndex);
        PlayerPrefs.Save();
    }

    public void SetVSync(bool isEnabled)
    {
        QualitySettings.vSyncCount = isEnabled ? 1 : 0;
        PlayerPrefs.SetInt("VSync", isEnabled ? 1 : 0);
        PlayerPrefs.Save();
    }

    // public void SetShadows(int shadowIndex)
    // {
    //     QualitySettings.shadows = (ShadowQuality)shadowIndex;
    //     PlayerPrefs.SetInt("Shadows", shadowIndex);
    //     PlayerPrefs.Save();
    // }

    // public void SetDrawDistance(int distanceIndex)
    // {
    //     float[] distances = { 300f, 500f, 750f, 1000f, 2000f };
    //     if (distanceIndex < 0 || distanceIndex >= distances.Length)
    //     {
    //         Debug.LogWarning("Índice de distancia de dibujo inválido.");
    //         return;
    //     }

    //     Camera.main.farClipPlane = distances[distanceIndex];
    //     PlayerPrefs.SetInt("DrawDistance", distanceIndex);
    //     PlayerPrefs.Save();
    // }

    public void SetFPSLimit(int index)
    {
        int[] fpsLimits = { 30, 60, 120, 144, 165 -1 };
        if (index < 0 || index >= fpsLimits.Length)
        {
            Debug.LogWarning("Índice de límite de FPS inválido.");
            return;
        }

        Application.targetFrameRate = fpsLimits[index];
        PlayerPrefs.SetInt("FPSLimit", index);
        PlayerPrefs.Save();
    }

    public void LoadSettings()
    {

        if (/*qualityDropdown == null ||*/ resolutionDropdown == null || screenModeDropdown == null || 
            vsyncToggle == null || fpsLimitDropdown == null) //|| shadowsDropdown == null || drawDistanceDropdown == null)
        {
            Debug.LogError("Uno o más elementos UI no están asignados en el Inspector.");
            return;
        }

        //qualityDropdown.value = PlayerPrefs.GetInt("Quality", 2);
        screenModeDropdown.value = PlayerPrefs.GetInt("ScreenMode", 0);
        vsyncToggle.isOn = PlayerPrefs.GetInt("VSync", 1) == 1;
        // shadowsDropdown.value = PlayerPrefs.GetInt("Shadows", 2);
        // drawDistanceDropdown.value = PlayerPrefs.GetInt("DrawDistance", 1);
        fpsLimitDropdown.value = PlayerPrefs.GetInt("FPSLimit", 1);

        resolutionDropdown.onValueChanged.RemoveAllListeners();
        resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionIndex", 0);
        resolutionDropdown.RefreshShownValue();
        resolutionDropdown.onValueChanged.AddListener(SetResolution);

        //qualityDropdown.RefreshShownValue();
        screenModeDropdown.RefreshShownValue();
        // shadowsDropdown.RefreshShownValue();
        // drawDistanceDropdown.RefreshShownValue();
        fpsLimitDropdown.RefreshShownValue();
    }
}