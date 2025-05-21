using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialsEndDoorActivator : MonoBehaviour
{
    public GameObject tutorialsEndDoor;
    void OnDestroy()
    {
        if (!tutorialsEndDoor.activeSelf)
        {
            tutorialsEndDoor.SetActive(true);
        }
    }
}
