using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialsEndDoorActivator : MonoBehaviour
{
    public GameObject tutorialsEndDoor;

    public Animator tutorialsEndDoorAnimator;

    // Cuando se destruya la torre del tutorial:
    // 1. Activamos el collider que guardara partida 
    // 2. Activamos el animator de la puerta
    void OnDestroy()
    {
        if (!tutorialsEndDoor.activeSelf)
        {
            tutorialsEndDoor.SetActive(true);
            if (tutorialsEndDoorAnimator.enabled == false)
            {
                Debug.Log("--->>>>> ANIMACION PUERTA ACTIVADA");
                tutorialsEndDoorAnimator.enabled = true;
            }
        }
    }
}
