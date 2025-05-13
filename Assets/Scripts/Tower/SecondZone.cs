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

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.enemy))
        {
            enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                if (enemy.activeElement == tower.activeElement)
                {
                    tower.enemiesInSecondZoneRange.Add(enemy.gameObject);
                    tower.secondZoneContact = true;
                    enemy.tower = tower;
                    enemyCount++;
                }
            }
            else 
            {
                Debug.Log("---->>>> NO EXISTE EL COMPONENTE ENEMY");
            }
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
                    enemyCount--;
                    tower.secondZoneContact = false;
                    tower.enemiesInSecondZoneRange.Remove(enemy.gameObject);

                }
            }
        }
    }
}
