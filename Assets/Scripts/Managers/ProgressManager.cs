using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor;

// Objeto a guardar en el JSON y que contiene todas la data
public class ProgressData
{
    public bool tutorial;
}

public class ProgressManager : MonoBehaviour
{
    public static ProgressManager Instance { get; private set; }
    ProgressData progressData;
    string saveFilePath;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Aseguramos que el gameobject no se destruye al cargar la escena
        }
        else
        {
            Destroy(gameObject); // Destruimos la nueva instancia si ya existe
        }
    }

    void Start()
    {
        progressData = new ProgressData(); // Instancia del ProgressData
        progressData.tutorial = true;
  
        saveFilePath = Application.persistentDataPath + "/ProgressManager.json"; // Ruta del JSON

        if (File.Exists(saveFilePath))
        {
            string loadProgressData = File.ReadAllText(saveFilePath);
            progressData = JsonUtility.FromJson<ProgressData>(loadProgressData);
        }
        else
        {
            string savePlayerData = JsonUtility.ToJson(progressData);
            File.WriteAllText(saveFilePath, savePlayerData);
            Debug.Log("Save file created at: " + saveFilePath);
        }
    }
    public void SaveGame()
    {
        string savePlayerData = JsonUtility.ToJson(progressData);
        File.WriteAllText(saveFilePath, savePlayerData);
        Debug.Log("Save file created at: " + saveFilePath);
    }
    public void LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string loadProgressData = File.ReadAllText(saveFilePath);
            progressData = JsonUtility.FromJson<ProgressData>(loadProgressData);
        }
        else
        {
            Debug.Log("There is no save files to load!");
        }
    }
    public void DeleteSaveFile()
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);  
            Debug.Log("Save file deleted!");
        }
        else
        {
            Debug.Log("There is nothing to delete!");
        }
    }
}
