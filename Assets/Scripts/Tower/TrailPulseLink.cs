using System.Collections;
using UnityEngine;

public class TrailPulseLink : MonoBehaviour
{
    public Transform[] enemiesList;
    public GameObject trailPrefab;
    public float pulseInterval = 1.5f;

    private float timer;

    void Update()
    {
        if (enemiesList == null || trailPrefab == null || enemiesList.Length == 0)
            return;

        timer += Time.deltaTime;
        if (timer >= pulseInterval)
        {
            timer = 0f;

            foreach (Transform enemy in enemiesList)
            {
                if (enemy != null)
                    StartCoroutine(LaunchTrail(enemy));
            }
        }
    }

    IEnumerator LaunchTrail(Transform enemy)
    {
         GameObject trail = Instantiate(trailPrefab, transform.position + Vector3.up * 2, Quaternion.identity);

    float duration = 0.5f;
    float elapsed = 0f;

    while (elapsed < duration)
    {
        float t = elapsed / duration;

        Vector3 start = transform.position + Vector3.up * 2;
        Vector3 currentTarget = enemy.position + Vector3.up * 1.5f;

        Vector3 mid = Vector3.Lerp(start, currentTarget, t);
        mid.y += Mathf.Sin(t * Mathf.PI) * 2f;

        trail.transform.position = mid;

        elapsed += Time.deltaTime;
        yield return null;
        }

        Destroy(trail);
    }
}
