using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveGamePanel : MonoBehaviour
{
    public Animator animator; // Animator de BoxInfo
    public void ShowPanel()
    {
        Debug.Log("SaveGameAnimation Animation");

        animator.Play("Show");
    }
}
