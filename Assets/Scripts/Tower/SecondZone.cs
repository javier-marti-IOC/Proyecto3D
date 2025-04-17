using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SecondZone : MonoBehaviour
{
    public Tower tower;
    public List<GameObject> instantiatedEnemies = new List<GameObject>();
    public int enemyCount;

    public void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {   
            TowerEnemy enemy = other.GetComponent<TowerEnemy>();
            if(enemy != null)
            {
                if (enemy.selectedType == tower.selectedType)
                {
                    tower.secondZoneContact = true;
                    // Debug.Log("CONTACTO ENEMIGO DE MI ELEMENTO");
                }
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {   
            TowerEnemy enemy = other.GetComponent<TowerEnemy>();
            if(enemy != null)
            {
                if (enemy.selectedType == tower.selectedType)
                {
                    enemyCount = enemyCount + 1;
                }
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {   
            TowerEnemy enemy = other.GetComponent<TowerEnemy>();
            if(enemy != null)
            {
                if (enemy.selectedType == tower.selectedType)
                {
                    enemyCount = enemyCount - 1;
                    tower.secondZoneContact = false;
                }
            }
        }
    }
}
