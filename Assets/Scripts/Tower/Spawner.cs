using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // Array que contiene los 4 tipos de enmigos
    public Tower tower; // Referencia a la torre

    public SecondZone secondZone;
    public void SpawnEnemy(int pos)
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
    }
}
