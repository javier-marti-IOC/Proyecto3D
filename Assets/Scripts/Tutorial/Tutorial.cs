using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [Header("Progress Manager")]
    public ProgressManager progressManager;
    public ProgressData progressData;
    public GameObject tutorialsEndDoor;

    void Start()
    {
        if (ProgressManager.Instance.Data.towerActiveElements.Contains(Element.Tutorial))
        {

        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Comparamos el tag 
        {
            ProgressManager.Instance.Data.tutorial = true;
            progressManager.SaveGame();
            Debug.Log("----->>>>> TUTORIAL COMPLETADO");
        }
    }
}
