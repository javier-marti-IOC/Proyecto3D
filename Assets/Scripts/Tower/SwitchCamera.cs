using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCamera : MonoBehaviour
{
    [Header("Cameras")]
    public GameObject main_camera;
    public GameObject secondary_camera;
    [Header("FadeIn / FadeOut")]
    public Animator animator;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            GetComponent<Animator>().SetTrigger("Change");
        }
    }

    // Funcion para switchear las camaras
    public void ToggleCam()
    {
        if (main_camera.activeSelf)
        {
            main_camera.SetActive(false);
            secondary_camera.SetActive(true);
        }
        else if(secondary_camera.activeSelf)
        {
            main_camera.SetActive(true);
            secondary_camera.SetActive(false);
        }
    }
}
