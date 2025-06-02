using System.Collections.Generic;
using UnityEngine;

public class TrailPulseLink : MonoBehaviour
{
    public List<Transform> enemiesList;
    public GameObject trailPrefab;
    public float pulseInterval = 1.5f;

    private float timer;

    [Header("Torre")]
    public Tower tower;

    private class TrailData
    {
        public GameObject trail;
        public Transform enemy;
        public float elapsed = 0f;
        public float duration = 0.5f;
    }

    private List<TrailData> activeTrails = new List<TrailData>();

    void Update()
    {
        if (tower == null || tower.enemiesInSecondZoneRange == null || trailPrefab == null || tower.enemiesInSecondZoneRange.Count == 0)
            return;

        timer += Time.deltaTime;
        if (timer >= pulseInterval)
        {
            timer = 0f;

            foreach (GameObject enemy in tower.enemiesInSecondZoneRange)
            {
                if (enemy != null)
                {
                    LaunchTrail(enemy.transform);
                }
            }
        }

        // Actualiza los trails activos
        for (int i = activeTrails.Count - 1; i >= 0; i--)
        {
            TrailData data = activeTrails[i];

            // ⚠️ Verifica si el enemigo fue destruido
            if (data.enemy == null)
            {
                if (data.trail != null)
                    Destroy(data.trail);

                activeTrails.RemoveAt(i);
                continue;
            }

            data.elapsed += Time.deltaTime;
            float t = data.elapsed / data.duration;

            if (t >= 1f)
            {
                if (data.trail != null)
                    Destroy(data.trail);

                activeTrails.RemoveAt(i);
                continue;
            }

            Vector3 start = transform.position + Vector3.up * 0.5f;
            Vector3 target = data.enemy.position + Vector3.up * 1.5f;
            Vector3 mid = Vector3.Lerp(start, target, t);
            mid.y += Mathf.Sin(t * Mathf.PI) * 2f;

            if (data.trail != null)
                data.trail.transform.position = mid;
        }
    }

    void LaunchTrail(Transform enemy)
    {
        GameObject trail = Instantiate(trailPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity, enemy);
        TrailData data = new TrailData { trail = trail, enemy = enemy };
        activeTrails.Add(data);
    }
}
