using UnityEngine;

public class OrbDropSpawner : MonoBehaviour
{
    public enum TrajectoryType
    {
        StraightDown,
        Parabola,
        ForwardArc
    }

    [Header("Orbe")]
    public GameObject orbPrefab;

    [Header("Opciones de Trayectoria")]
    public TrajectoryType trajectory = TrajectoryType.StraightDown;
    public float speed = 5f;
    public float angleInDegrees = 45f;
    public float randomSpread = 0.5f;

    [Header("Gravedad (solo parabólica)")]
    public float gravity = -9.81f;

    [Header("Auto Spawn")]
    public bool autoSpawn = true;
    public float spawnInterval = 5f; // <-- Cada 5 segundos
    private float timer = 0f;

    void Update()
    {
        if (!autoSpawn) return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnOrb();
            timer = 0f;
        }
    }

    public void SpawnOrb()
    {
        if (orbPrefab == null)
        {
            Debug.LogWarning("No se asignó el prefab del orbe.");
            return;
        }

        GameObject orb = Instantiate(orbPrefab, transform.position, Quaternion.identity);

        Rigidbody rb = orb.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = orb.AddComponent<Rigidbody>();
        }

        switch (trajectory)
        {
            case TrajectoryType.StraightDown:
                rb.velocity = Vector3.down * speed;
                break;

            case TrajectoryType.Parabola:
                rb.useGravity = true;
                float rad = angleInDegrees * Mathf.Deg2Rad;
                Vector3 direction = Quaternion.Euler(0, Random.Range(-randomSpread, randomSpread) * 30f, 0) * transform.forward;
                Vector3 velocity = direction * Mathf.Cos(rad) * speed + Vector3.up * Mathf.Sin(rad) * speed;
                rb.velocity = velocity;
                break;

            case TrajectoryType.ForwardArc:
                rb.useGravity = false;
                Vector3 arcDirection = (transform.forward + Vector3.down).normalized;
                rb.velocity = arcDirection * speed;
                break;
        }
    }
}
