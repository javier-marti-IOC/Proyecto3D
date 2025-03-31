using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrincipalMenu : MonoBehaviour
{
    public void quitGame()
    {
        Debug.Log("Sortir del joc");
        Application.Quit();
    }
}
