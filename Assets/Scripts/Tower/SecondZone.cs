using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SecondZone : MonoBehaviour
{
    public Tower tower;
    public Enemy enemy;
    public List<Enemy> instantiatedEnemies = new List<Enemy>();
    public int enemyCount;
    public bool isCalling;


    /* public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(Constants.enemy))
        {
            enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                if (enemy.activeElement == tower.activeElement)
                {
                    tower.secondZoneContact = true;
                    // Debug.Log("CONTACTO ENEMIGO DE MI ELEMENTO");
                }
            }
        }
    } */

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.enemy))
        {
            enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                if (enemy.activeElement == tower.activeElement)
                {
                    tower.secondZoneContact = true;
                    tower.CallAllEnemies(instantiatedEnemies, true);
                    enemyCount = enemyCount + 1;

                }
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Constants.enemy))
        {
            enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                if (enemy.activeElement == tower.activeElement)
                {
                    enemyCount = enemyCount - 1;
                    tower.secondZoneContact = false;

                    enemy.towerCalling = false;

                }
            }
        }
    }
}
