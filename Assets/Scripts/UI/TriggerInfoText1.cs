using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerInfoText1 : MonoBehaviour
{
    public int num;
    public GameManager gameManager;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.player))
        {
            if (num == 1)
            {
                gameManager.EnterRollHelp();
            }
            else if (num == 2)
            {
                gameManager.EnterAttackHelp();
            }
        }        
    }
}
