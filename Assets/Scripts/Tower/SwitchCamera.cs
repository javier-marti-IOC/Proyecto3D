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
        if(Input.GetKey(KeyCode.I))
        {
            FadeIn();
            /* if(!secondary_camera.activeSelf)
            {
                FadeIn();
                //ToggleCamera(1);
                Invoke("ActivateSecondaryCam", 1f);
            } */
        }

        if(Input.GetKey(KeyCode.U))
        {
            FadeOut();
            /* if(!main_camera.activeSelf)
            {
                ToggleCamera(0);
                FadeOut();
            } */
        }
    }

    public void ActivateMainCam()
    {
        main_camera.SetActive(true);
        secondary_camera.SetActive(false);
    }

    public void ActivateSecondaryCam()
    {
        main_camera.SetActive(false);
        secondary_camera.SetActive(true);
    }

    // Toggle camaras
    public void ToggleCamera(int cam)
    {
        if(cam == 1)
        {
            main_camera.SetActive(true);
            secondary_camera.SetActive(false);
            return;
        }
        main_camera.SetActive(false);
        secondary_camera.SetActive(true);
    }

    public void FadeOut()
    {
       animator.Play("FadeOut"); 
    }

    public void FadeIn()
    {
       animator.Play("FadeIn"); 
    }
}
