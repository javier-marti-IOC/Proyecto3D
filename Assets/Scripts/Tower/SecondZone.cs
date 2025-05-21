using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SecondZone : MonoBehaviour
{
    public Tower tower;
    public Enemy enemy;
    public bool playerInSecondZoneRange;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.enemy))
        {
            enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                if (enemy.activeElement == tower.activeElement)
                {
                    enemy.towerInRange = true;
                    tower.enemiesInSecondZoneRange.Add(enemy.gameObject);
                    enemy.tower = tower;
                    tower.CheckSecondZoneCount(tower.enemiesInSecondZoneRange);
                }
            }
            else
            {
                Debug.Log("---->>>> NO EXISTE EL COMPONENTE ENEMY");
            }
        }

        if (other.CompareTag(Constants.player))
        {
            playerInSecondZoneRange = true;
            // Debug.Log("---->>>>>>> PLAYER EN ZONA");
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Constants.enemy))
        {
            enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                if (enemy.activeElement == tower.activeElement)
                {
                    enemy.towerInRange = false;
                    tower.enemiesInSecondZoneRange.Remove(enemy.gameObject);
                    tower.CheckSecondZoneCount(tower.enemiesInSecondZoneRange);
                }
            }
        }

        if (other.CompareTag(Constants.player))
        {
            playerInSecondZoneRange = false;
            // Debug.Log("---->>>>>>> PLAYER FUERA DE ZONA");
        }
    }
}
