using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [Header("Progress Manager")]
    public ProgressManager progressManager;
    public ProgressData progressData;
    public GameObject tutorialsEndDoor;
    public GameObject tutorialsTower;

    /* void Start()
    {
        if (tutorialsTower == null)
        {
            tutorialsTower = GameObject.Find("TutorialTower");
        }
        if (tutorialsTower.activeSelf == false)
        {
            tutorialsTower.SetActive(true);
            if (ProgressManager.Instance.Data.towerActiveElements.Contains(Element.Tutorial))
            {
                Debug.Log("*************** TUTORIAL DESTRUIDO");
                tutorialsEndDoor.GetComponent<Animator>().enabled = false;
                tutorialsEndDoor.transform.rotation = Quaternion.Euler(0f, -144.157f, 0f);
            }
        }
    } */

    void Update()
    {
        if (ProgressManager.Instance.Data.towerActiveElements.Contains(Element.None))
        {
            tutorialsEndDoor.GetComponent<Animator>().enabled = false;
            tutorialsEndDoor.transform.rotation = Quaternion.Euler(0f, -144.157f, 0f);
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
