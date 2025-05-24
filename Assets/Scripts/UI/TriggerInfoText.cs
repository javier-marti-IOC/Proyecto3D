using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerInfoText : MonoBehaviour
{
    public string textInfo;
    public InfoPanelHUD infoPanelHUD;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.player))
        {
                infoPanelHUD.ShowText(textInfo);
        }        
    }
}
