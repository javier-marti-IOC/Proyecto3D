using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MagePillar : MonoBehaviour
{

    public int phrasePosition;
    public string[] mageAdvices = new string[] { };
    public GameObject magePanel;
    public TextMeshProUGUI mageAdviceText;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.player))
        {
            if (!magePanel.activeSelf)
            {
                magePanel.SetActive(true);
                if (phrasePosition >= 0 && phrasePosition <= mageAdvices.Length)
                {
                    mageAdviceText.text = mageAdvices[phrasePosition];
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Constants.player))
        {
            if (magePanel.activeSelf)
            {
                magePanel.SetActive(false);
            }
        }
    }
}
