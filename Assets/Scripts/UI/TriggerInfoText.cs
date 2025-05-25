using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerInfoText : MonoBehaviour
{
    public string textInfo;
    public InfoPanelHUD infoPanelHUD;
    public bool activated;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.player))
        {
            if (activated == false)
            {
                infoPanelHUD.ShowText(textInfo);
                activated = true;  
            }
            
        }        
    }
}
