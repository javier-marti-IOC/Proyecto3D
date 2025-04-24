using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor;

public class ProgressManager : MonoBehaviour
{

    public static ProgressManager Instance { get; private set; }

    private string filePath;

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
        filePath = Application.dataPath + "/ProgressManager.json";
        if (File.Exists(filePath))
        {
            Debug.Log("------> ARCHIVO EXISTENTE");
        }
        else
        {
            WriteData();
        }
    }

    public void WriteData()
    {
        string word = "¡Hello World!";
        File.WriteAllText(Application.dataPath +"/ProgressManager.json", word);
        Debug.Log("------> ARCHIVO CREADO");

    }
}
