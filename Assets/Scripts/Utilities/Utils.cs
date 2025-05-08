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
}
