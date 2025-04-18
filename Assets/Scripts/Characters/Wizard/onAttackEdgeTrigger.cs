using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onAttackEdgeTrigger : MonoBehaviour
{
    public wizardBT wizardBT;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            Debug.Log("OnInerTriggerENTER");
            wizardBT.OnInerTriggerEnter();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            Debug.Log("OnInerTriggerEXIT");
            wizardBT.OnInerTriggerExit();
        }
    }
}
