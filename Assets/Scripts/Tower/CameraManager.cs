using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Cameras")]
    public GameObject main_camera;
    public GameObject secondary_camera;
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
    public void ActivateFade()
    {
        GetComponent<Animator>().SetTrigger("Change");
    }
}
