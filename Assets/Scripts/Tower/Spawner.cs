using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject[] enemyPrefabs; // Array que contiene los 4 tipos de enmigos
    public Tower tower; // Referencia a la torre

    public SecondZone secondZone;
    [Header("Track points")]
    public Transform[] trackPoints;  // Array que contiene los track points donde aparecen los enemigos
    private int rndNum;
    void Start()
    {
        /* if(trackPoints.Length > 0)
        {
            rndNum = Random.Range(0, trackPoints.Length);

            Debug.Log("TRK POINT NAME: " + trackPoints[rndNum].name); 
            Debug.Log("TRK POINT X POSITION: " + trackPoints[rndNum].transform.localPosition.x); 
        }
        else
        {
            Debug.Log("No hay track points");
        } */
    }

    public void SpawnEnemy(Element activeElement)
    {
        if (trackPoints.Length > 0)
        {
            rndNum = Random.Range(0, trackPoints.Length); // Seleccionamos un track point aleatorio dentro del array

            if (tower.enemiesInSecondZoneRange.Count < 3) // Verificamos los enemigos instanciados en la zona 
            {
                foreach (GameObject enemy in enemyPrefabs) // Recorremos el array que contiene los 4 tipos de enemigos
                {
                    if (enemy.transform)
                    {
                        if (enemy.transform.GetChild(0).transform.GetComponent<Enemy>().activeElement == activeElement) // Verificamos que el activeElement del enemigo concuerde con el de la funcion
                        {
                            // Debug.Log("-----> INSTANCIO");
                            if (trackPoints.Length > 0)
                            {
                                GameObject newEnemy = Instantiate(enemy, trackPoints[rndNum].transform.position, Quaternion.identity,gameManager.transform); // Instanciamos el enemigo
                                newEnemy.GetComponentInChildren<Enemy>().tower = tower;
                                if (activeElement == Element.Earth)
                                {
                                    newEnemy.GetComponentInChildren<Enemy>().enemyLevel = gameManager.earthLevel;
                                }
                                else if (activeElement == Element.Water)
                                {
                                    newEnemy.GetComponentInChildren<Enemy>().enemyLevel = gameManager.waterLevel;
                                }
                                else if (activeElement == Element.Fire)
                                {
                                    newEnemy.GetComponentInChildren<Enemy>().enemyLevel = gameManager.fireLevel;
                                }
                                else if (activeElement == Element.Electric)
                                {
                                    newEnemy.GetComponentInChildren<Enemy>().enemyLevel = gameManager.electricLevel;
                                }
                                newEnemy.GetComponentInChildren<Enemy>().SetStatsByLevel();
                                tower.isOnCooldown = true; // Activamos el cooldown   
                            }    
                        }
                    }
                }
            }
            else
            {
                // Debug.Log("NO PUEDO CREAR TANTOS ENEMIGOS");
            }
        }
    }































    /* public void SpawnEnemy(int pos)
    {
        int spawnPointX = Random.Range(36, 38); // Coordenada X
        int spawnPointY = 5; // Coordenada Y
        int spawnPointZ = Random.Range(115, 122); // Coordenada Z

        Vector3 spawnPosition = new Vector3(spawnPointX, spawnPointY, spawnPointZ); // Posicion

        Debug.Log("----------> NUMERO DE ENEMIGOS INSTANCIADOS EN ZONA: " + secondZone.instantiatedEnemies.Count);
        
        if(secondZone.enemyCount < 6)
        {
            Instantiate(enemyPrefabs[pos], spawnPosition, Quaternion.identity); // Instanciamos el enemigo
            tower.isOnCooldown = true; // Activamos el cooldown
        }
        else
        {
            Debug.Log("NO PUEDO CREAR TANTOS ENEMIGOS");
        }
    } */

    /* public void SpawnEnemy(int pos, Collider spawnZone, float y_position)
    {
        Vector3 spawnPosition = Vector3.zero;
        if (spawnZone is BoxCollider box)
        {
            Vector3 center = box.center + box.transform.position;
            Vector3 size = box.size;
            Vector3 randomPosition = new Vector3(
                Random.Range(-size.x / 2, size.x / 2),
                // Random.Range(-size.y / 2, size.y / 2),
                y_position,
                Random.Range(-size.z / 2, size.z / 2)
            );
            spawnPosition = box.transform.TransformPoint(randomPosition);
        }
        Debug.Log("----------> NUMERO DE ENEMIGOS INSTANCIADOS EN ZONA: " + secondZone.instantiatedEnemies.Count);
        if (secondZone.enemyCount < 6)
        {
            Instantiate(enemyPrefabs[pos], spawnPosition, Quaternion.identity);
            tower.isOnCooldown = true;
        }
        else
        {
            Debug.Log("NO PUEDO CREAR TANTOS ENEMIGOS");
        }
    } */
}
