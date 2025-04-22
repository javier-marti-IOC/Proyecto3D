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
                    tower.enemiesInSecondZoneRange.Add(enemy.GetComponentInParent<Transform>().gameObject);
                    tower.secondZoneContact = true;
                    enemy.tower = tower;
                    enemyCount++;
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
                    enemyCount--;
                    tower.secondZoneContact = false;
                    tower.enemiesInSecondZoneRange.Remove(enemy.GetComponentInParent<Transform>().gameObject);

                }
            }
        }
    }
}
