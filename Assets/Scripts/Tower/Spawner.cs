using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // Array que contiene los 4 tipos de enmigos
    public Tower tower; // Referencia a la torre

    public SecondZone secondZone;
    [Header("Track points")]
    public GameObject[] trackPoints;  // Array que contiene los track points donde aparecen los enemigos
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
    
    public void SpawnEnemy(int pos)
    {
        if(trackPoints.Length > 0)
        {
            rndNum = Random.Range(0, trackPoints.Length);
            
            int spawnPointX = (int) trackPoints[rndNum].transform.position.x; // Coordenada X
            int spawnPointY = (int) trackPoints[rndNum].transform.position.y; // Coordenada Y
            int spawnPointZ = (int) trackPoints[rndNum].transform.position.z; // Coordenada Z

            Vector3 spawnPosition = new Vector3(spawnPointX, spawnPointY, spawnPointZ); // Posicion

            // Debug.Log("NUEVA POSICION DE RESPAWN: " + spawnPointX + " " + spawnPointY + " " + spawnPointZ);
            // Debug.Log("----------> NUMERO DE ENEMIGOS INSTANCIADOS EN ZONA: " + secondZone.instantiatedEnemies.Count);
            
            if(secondZone.enemyCount < 6)
            {
                Instantiate(enemyPrefabs[pos], spawnPosition, Quaternion.identity); // Instanciamos el enemigo
                tower.isOnCooldown = true; // Activamos el cooldown
            }
            else
            {
                Debug.Log("NO PUEDO CREAR TANTOS ENEMIGOS");
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
