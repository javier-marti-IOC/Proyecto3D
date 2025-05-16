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
    public static void DestroyParent(GameObject child)
    {
        if (child.transform.parent != null)
        {
            Object.Destroy(child.transform.parent.gameObject);
        }
    }

}
