using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifePlantsSpawns : MonoBehaviour
{
    public Transform[] spawners;
    public GameObject lifeOrbSpawnerPrefab;
    void Start()
    {
        if (spawners.Length > 0)
        {
            foreach (Transform spawn in spawners)
            {
                Instantiate(lifeOrbSpawnerPrefab, spawn.position, Quaternion.identity);
            }
        }
    }
}
