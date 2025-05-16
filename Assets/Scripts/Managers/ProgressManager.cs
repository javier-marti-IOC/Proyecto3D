using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor;
using UnityEngine.SceneManagement;

// Objeto a guardar en el JSON y que contiene todas la data
public class ProgressData
{
    public bool tutorial;
    public List<Element> towerActiveElements;
}

public class ProgressManager : MonoBehaviour
{
    public ProgressData Data { get => progressData; set => progressData = value; }
    public static ProgressManager Instance { get; private set; }
    public ProgressData progressData;
    private string saveFilePath;

    private void Awake()
{
    if(Instance == null)
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    else
    {
        Destroy(gameObject);
        return;
    }

    // Mueve la inicialización del progreso aquí
    saveFilePath = Application.persistentDataPath + "/ProgressManager.json";

    if (!File.Exists(saveFilePath))
    {
        progressData = new ProgressData { tutorial = false, towerActiveElements = new List<Element>() };
    }
    else
    {
        string loadProgressData = File.ReadAllText(saveFilePath);
        progressData = JsonUtility.FromJson<ProgressData>(loadProgressData);
    }

    if (progressData == null)
    {
        progressData = new ProgressData { tutorial = false, towerActiveElements = new List<Element>() };
    }
}


    void Start()
    {
        // Guardamos la ruta donde queremos generar el JSON
        saveFilePath = Application.persistentDataPath + "/ProgressManager.json";
        // Si el archivo no existe, generamos el objeto del JSON con todo a 0
        if(!File.Exists(saveFilePath))
        {
            progressData = new ProgressData { tutorial = false, towerActiveElements = new List<Element>() };
        }
        else
        {
            // Si existe, lo leemos y cargamos los datos
            string loadProgressData = File.ReadAllText(saveFilePath);
            // Almacenamos los datos en progressData
            progressData = JsonUtility.FromJson<ProgressData>(loadProgressData);
        }
        // Verificar si progressData es nulo después de cargar
        if (progressData == null)
        {
            progressData = new ProgressData { tutorial = false, towerActiveElements = new List<Element>() };
        }
    }

    void Update()
    {
        // Guardar data
        if(Input.GetKeyDown(KeyCode.X))
        {
            SaveGame();
        }
        // Cargar data
        if(Input.GetKeyDown(KeyCode.Z))
        {
            if (File.Exists(saveFilePath))
            {
                string loadProgressData = File.ReadAllText(saveFilePath);
                progressData = JsonUtility.FromJson<ProgressData>(loadProgressData);
                Debug.Log("DATA GUARDADA: " + loadProgressData);
            }
            else
            {
                Debug.Log("There is no save files to load!");
            }
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            if (File.Exists(saveFilePath))
            {
                DeleteSavedFile();
            }
        }
    }



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
    }
    // Funcion para cargar partida
    public void LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string loadProgressData = File.ReadAllText(saveFilePath);
            progressData = JsonUtility.FromJson<ProgressData>(loadProgressData);
            SceneManager.LoadScene(1);
            Debug.Log("Partida cargada con exito!");
        }
        else
        {
            Debug.Log("There is no save files to load!");
        }
    }
    // Funcion para iniciar nueva partida
    public void DeleteSavedFile()
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);  
            Debug.Log("Save file deleted!");
            SceneManager.LoadScene(2);        
        }
        else
        {
            Debug.Log("There is nothing to delete!");
            SceneManager.LoadScene(2);
        }
    }
}





































/* void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            saveFilePath = Application.persistentDataPath + "/ProgressManager.json"; // Ruta del JSON
            Debug.Log(saveFilePath);

            if (File.Exists(saveFilePath))
            {
                string loadProgressData = File.ReadAllText(saveFilePath);
                progressData = JsonUtility.FromJson<ProgressData>(loadProgressData);
                Debug.Log("File existing");
            }
            else
            {
                string savePlayerData = JsonUtility.ToJson(progressData);
                File.WriteAllText(saveFilePath, savePlayerData);
                Debug.Log("Save file created at: " + saveFilePath);
            }
        }
    }  */