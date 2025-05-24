using System.Linq;
using UnityEngine;

public static class Utils
{
    public static void RotatePositionToTarget(Transform source, Transform target, float velocity)
    {
        // Rotar suavemente hacia el jugador
        Vector3 direction = (target.transform.position - source.position).normalized;
        direction.y = 0f; // Evitar inclinacion vertical

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        source.rotation = Quaternion.Slerp(source.rotation, lookRotation, Time.deltaTime * velocity); // 15 es la velocidad de giro
    }

    public static void ReplaceMaterials(Material[] sources, Color[] target)
    {
        for (int i = 0; i < sources.Length; i++)
        {
            sources[i].SetColor("_BaseColor", target[i]);
        }
    }
    public static void DestroyRoot(GameObject child)
    {
        if (child.transform.root != null)
        {
            Object.Destroy(child.transform.root.gameObject);
        }
    }

    public static void ReplaceTrees(GameObject[] trees, Mesh[] newTrees)
    {
        foreach (GameObject tree in trees)
        {
            int randPosition = Random.Range(0, newTrees.Length);
            tree.GetComponent<MeshFilter>().mesh = newTrees[randPosition];
            tree.GetComponent<MeshCollider>().sharedMesh = newTrees[randPosition];
        }
    }

    public static void DestroyCorruptedClouds(GameObject[] corruptedClouds)
    {
        if (corruptedClouds.Length > 0)
        {    
            foreach (GameObject corruptedCloud in corruptedClouds)
            {
                corruptedCloud.SetActive(false);
            }
        }
    }

}
