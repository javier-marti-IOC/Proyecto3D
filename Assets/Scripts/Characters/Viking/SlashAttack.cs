using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashAttack : MonoBehaviour
{
    public VikingController vikingController;

    [Header("Slash Growth Settings")]
    public float maxScale = 3f;
    public float growthSpeed = 5f;

    private Vector3 initialScale;
    private bool isGrowing = true;

    void Start()
    {
        initialScale = transform.localScale;
    }

    void Update()
    {
        if (isGrowing)
        {
            GrowSlash();
        }
    }

    void GrowSlash()
    {
        // Increase only the scale in X (width) and Z (length), keeping Y (thickness) same or minimal
        Vector3 targetScale = new Vector3(maxScale, initialScale.y, initialScale.z);
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * growthSpeed);

        // Stop when sufficiently close
        if (Vector3.Distance(transform.localScale, targetScale) < 0.05f)
        {
            transform.localScale = targetScale;
            isGrowing = false;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        vikingController.SlashAttackEnter(other);
    }
}
