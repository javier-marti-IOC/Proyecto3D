using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAppearence : MonoBehaviour
{
    // Funcion para cambiar el color de los objetos
    public void ToggleColor(Transform objects, Color objectColor)
    {

        foreach (Transform child in objects)
        {
            child.GetComponent<MeshRenderer>().material.color = objectColor;
        }
    }
    // Funcion para cambiar el material de los objetos
    public void ToggleMesh(Transform objects, Mesh newMesh)
    {

        foreach (Transform child in objects)
        {
            child.GetComponent<MeshFilter>().mesh = newMesh;
        }
    }
}
