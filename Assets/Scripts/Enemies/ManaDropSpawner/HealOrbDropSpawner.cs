using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealOrbDropSpawner : MonoBehaviour
{
    [Header("Planta")]
    public int life;
    public int damageAmount;

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

    [Header("Coordenadas de spawn")]
    public Transform lifeOrbSpawnPosition;

    [Header("Particulas")]
    public GameObject hitParticleEffect;

    /* [Header("Auto Spawn")]
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
    } */

    void Start()
    {
        if (hitParticleEffect != null)
        {
            hitParticleEffect.SetActive(false);
        }

        int num = Random.Range(2, 6);
        life = num;
    }
    void Update()
    {
        if (life <= 0)
        {
            AudioManager.Instance?.Play("DestroyMushroom");
            Destroy(gameObject);
        }
    }


    public void SpawnOrb()
    {
        if (orbPrefab == null)
        {
            Debug.LogWarning("No se asignó el prefab del orbe.");
            return;
        }

        GameObject orb = Instantiate(orbPrefab, lifeOrbSpawnPosition.position, Quaternion.identity);

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

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.sword))
        {
            SpawnOrb();
            hitParticleEffect.SetActive(false);

            hitParticleEffect.SetActive(true);
            life -= damageAmount;
            if (life != 0)
            {
                AudioManager.Instance?.Play("HitMushroom");
            }
        }
    }
}
