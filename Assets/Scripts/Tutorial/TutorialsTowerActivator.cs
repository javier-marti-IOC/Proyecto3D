using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialsTowerActivator : MonoBehaviour
{
    public GameObject tutorialsTower;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.player))
        {
            if (tutorialsTower)
            {
                tutorialsTower.SetActive(true);
            }
        }        
    }
}
