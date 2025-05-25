using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeDrops : MonoBehaviour
{
    public AudioSource audioOrbeBounce;
    public float minImpactVelocity = 0.2f;
    private float autoDestrucion = 10;
    void Update()
    {
        autoDestrucion -= Time.deltaTime;
        if (autoDestrucion < 0)
        {
            Destroy(gameObject);
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.player))
        {
            other.GetComponent<VikingController>().CollectLife();
            Destroy(gameObject);
        }
    }
    
      private void OnCollisionEnter(Collision collision)
    {
        // Comprobamos la velocidad justo antes del impacto
        float impactVelocity = collision.relativeVelocity.magnitude;

        if (impactVelocity > minImpactVelocity && audioOrbeBounce != null)
        {
            audioOrbeBounce.Play();
        }
    }
}
