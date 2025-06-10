using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Objeto a guardar en el JSON y que contiene todas la data
public class ProgressData
{
    public bool tutorial;
    public List<Element> towerActiveElements;
    public bool continuePlaying;

    public List<Element> GetTowerActiveElements()
    {
        return towerActiveElements;
    }
}

public class ProgressManager : MonoBehaviour
{
    public ProgressData Data { get => progressData; set => progressData = value; }
    public static ProgressManager Instance { get; private set; }
    public ProgressData progressData;
    private string saveFilePath;
    public GameObject continueBtn;
    public Button newGameBtn;

    private bool earthTowerDestroyed;
    private bool fireTowerDestroyed;
    private bool waterTowerDestroyed;
    private bool electricTowerDestroyed;

    public PauseMenu pauseMenu;
    private SaveGamePanel saveGamePanel;

    private void Awake()
    {


        // Mueve la inicialización del progreso aquí
        saveFilePath = Application.persistentDataPath + "/progressManager.json";

        if (!File.Exists(saveFilePath))
        {
            progressData = new ProgressData { tutorial = false, towerActiveElements = new List<Element>() };
        }
        else
        {
            LoadData();
        }

        if (progressData == null)
        {
            progressData = new ProgressData { tutorial = false, towerActiveElements = new List<Element>() };
        }
    }


    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        // Guardamos la ruta donde queremos generar el JSON
        saveFilePath = Application.persistentDataPath + "/progressManager.json";
        saveGamePanel = FindObjectOfType<SaveGamePanel>();
        // Si el archivo no existe, generamos el objeto del JSON con todo a 0
        if (!File.Exists(saveFilePath))
        {
            progressData = new ProgressData { tutorial = false, towerActiveElements = new List<Element>() };
            DeactivateContinueBtn();
        }
        else
        {
            // Si existe, lo leemos y cargamos los datos
            LoadData();
            ActivateContinueBtn();
        }
        // Verificar si progressData es nulo después de cargar
        if (progressData == null)
        {
            progressData = new ProgressData { tutorial = false, towerActiveElements = new List<Element>() };
        }
        checkDestroyedTowers();
        musicMenu();
        if (pauseMenu != null)
        {
            if (pauseMenu.endGamePanel.activeSelf)
            {
                pauseMenu.TogglePause();
                Debug.Log("Desactivado panel fin partida");
            }
        }
    }

    // void Update()
    // {
    //     // Guardar data
    //     if (Input.GetKeyDown(KeyCode.X))
    //     {
    //         SaveGame();
    //     }
    //     // Cargar data
    //     if (Input.GetKeyDown(KeyCode.Z))
    //     {
    //         if (File.Exists(saveFilePath))
    //         {
    //             LoadData();
    //         }
    //         else
    //         {
    //             Debug.Log("There is no save files to load!");
    //         }
    //     }

    //     if (Input.GetKeyDown(KeyCode.E))
    //     {
    //         if (File.Exists(saveFilePath))
    //         {
    //             DeleteSavedFile();
    //         }
    //     }
    // }



    // Funcion para guardar partida
    public void SaveGame()
    {
        if (progressData != null && progressData.towerActiveElements != null)
        {
            string savePlayerData = JsonUtility.ToJson(progressData);
            File.WriteAllText(saveFilePath, savePlayerData);
            Debug.Log("Save file created at: " + saveFilePath);
        }
        else
        {
            Debug.LogError("ProgressData or towerActiveElements are null, cannot save.");
        }
        if (saveGamePanel != null)
        {
            saveGamePanel.ShowPanel();
        }
    }
    // Funcion para cargar partida
    public void LoadGame()
    {
        LoadData();
        if (File.Exists(saveFilePath))
        {
            SceneManager.LoadScene(1);
            Debug.Log("Partida cargada con exito!");
        }
        else
        {
            Debug.Log("There is no save files to load!");
        }
    }
    public void LoadGameFromFinalScene()
    {
        ProgressManager.Instance.Data.continuePlaying = true;
        SaveGame();
        LoadData();
        if (File.Exists(saveFilePath))
        {
            SceneManager.LoadScene(1);
            Debug.Log("Partida cargada con exito!");
        }
        else
        {
            Debug.Log("There is no save files to load!");
        }
    }

    public void exitToMenuFinalScene()
    {
        ProgressManager.Instance.Data.continuePlaying = true;
        SaveGame();
        LoadData();
        SceneManager.LoadScene(0);
    }
    // Funcion para iniciar nueva partida
    public void DeleteSavedFile()
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
            Debug.Log("Save file deleted!");
            SceneManager.LoadScene(1);
        }
        else
        {
            Debug.Log("There is nothing to delete!");
            SceneManager.LoadScene(1);
        }
    }
    // Funcion para activar el boton de continuar del menu de inicio
    public void ActivateContinueBtn()
    {
        if (continueBtn != null)
        {
            continueBtn.SetActive(true);
        }
    }
    // Funcion para desactivar el boton de continuar del menu de inicio
    public void DeactivateContinueBtn()
    {
        if (continueBtn != null)
        {
            continueBtn.SetActive(false);
            newGameBtn.Select();
        }
    }
    public bool LoadData()
    {
        saveFilePath = Application.persistentDataPath + "/progressManager.json";
        if (!File.Exists(saveFilePath))
        {
            Debug.Log("NO FILEEE");
            return false;
        }
        string loadProgressData = File.ReadAllText(saveFilePath);
        progressData = JsonUtility.FromJson<ProgressData>(loadProgressData);
        Debug.Log("DATA CARGADA: " + loadProgressData + " Ubication: " + saveFilePath);
        return true;
    }
    public void checkDestroyedTowers()
    {
        if (progressData.towerActiveElements.Count > 0)
        {
            for (int i = 0; i < progressData.towerActiveElements.Count; i++)
            {
                if (progressData.towerActiveElements[i] == Element.Earth)
                {
                    //Debug.Log("-->>>> TORRE DE TIERRA ENCONTRADA");
                    earthTowerDestroyed = true;
                }
                if (progressData.towerActiveElements[i] == Element.Fire)
                {
                    //Debug.Log("-->>>> TORRE DE TIERRA ENCONTRADA");
                    fireTowerDestroyed = true;
                }
                if (progressData.towerActiveElements[i] == Element.Water)
                {

                    //Debug.Log("-->>>> TORRE DE TIERRA ENCONTRADA");
                    waterTowerDestroyed = true;
                }
                if (progressData.towerActiveElements[i] == Element.Electric)
                {

                    //Debug.Log("-->>>> TORRE DE TIERRA ENCONTRADA");
                    electricTowerDestroyed = true;
                }
            }
            if (earthTowerDestroyed && fireTowerDestroyed && waterTowerDestroyed && electricTowerDestroyed)
            {
                if (pauseMenu != null)
                {
                    //pauseMenu.ToggleEndgame();
                }
            }
        }
    }
    public void musicMenu()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0 && earthTowerDestroyed && fireTowerDestroyed && waterTowerDestroyed && electricTowerDestroyed)
        {
            AudioManager.Instance?.Play("menuMusicHealed");
        }
        else if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            AudioManager.Instance?.Play("menuMusicCorrupted");
        }
    }

    public bool IsEarthTowerDestroyed()
    {
        return earthTowerDestroyed;
    }
    public bool IsWaterTowerDestroyed()
    {
        return waterTowerDestroyed;
    }
    public bool IsFireTowerDestroyed()
    {
        return fireTowerDestroyed;
    }
    public bool IsElectricTowerDestroyed()
    {
        return electricTowerDestroyed;
    }
}