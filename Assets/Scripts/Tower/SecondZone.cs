using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SecondZone : MonoBehaviour
{
    public Tower tower;
    public Enemy enemy;
    public bool playerInSecondZoneRange;

    [Header("Elemental Mana Drop Spawners")]
    public GameObject[] elementalManaDropSpawners;

    // Activamos los spawners de orbes de mana
    public void activateElementalManaDropSpawners()
    {
        if (elementalManaDropSpawners.Length > 0)
        {
            foreach (GameObject manaDropSpawner in elementalManaDropSpawners)
            {
                if (!manaDropSpawner.activeSelf)
                {
                    manaDropSpawner.SetActive(true);
                }
            }
        }
    }
    // Desactivamos los spawners de orbes de mana
    public void deactivateElementalManaDropSpawners()
    {
        if (elementalManaDropSpawners.Length > 0)
        {    
            foreach (GameObject manaDropSpawner in elementalManaDropSpawners)
            {
                if (manaDropSpawner.activeSelf)
                {
                    manaDropSpawner.SetActive(false);
                }
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.enemy))
        {
            enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                if (enemy.activeElement == tower.activeElement)
                {
                    Debug.Log("TOWER ENRMY IN");
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
        else if (other.CompareTag(Constants.player))
        {
            playerInSecondZoneRange = true;
            activateElementalManaDropSpawners();
            foreach (GameObject e in tower.enemiesInSecondZoneRange)
            {
                e.GetComponent<Enemy>().PlayerDetected();
            }
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
                    Debug.Log("TOWER ENRMY OUT");
                    enemy.towerInRange = false;
                    tower.enemiesInSecondZoneRange.Remove(enemy.gameObject);
                    tower.CheckSecondZoneCount(tower.enemiesInSecondZoneRange);
                }
            }
        }

        if (other.CompareTag(Constants.player))
        {
            playerInSecondZoneRange = false;
            deactivateElementalManaDropSpawners();
            // Debug.Log("---->>>>>>> PLAYER FUERA DE ZONA");
        }
    }
}
