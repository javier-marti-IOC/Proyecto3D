using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagePillarSpawner : MonoBehaviour
{
    public GameObject magePrefab;
    public GameObject player;
    public GameObject mageParticle;
    public Transform[] magePillarsTrackPoints;
    public AudioSource audioSource;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag(Constants.player);
        }
        mageParticle.SetActive(false);
    }

    void Update()
    {
        if (magePrefab.activeSelf)
        {
            Vector3 direction = player.transform.position - magePrefab.transform.position;
            direction.y = 0; // Elimina la componente vertical para que solo rote en Y
            if (direction != Vector3.zero)
            {
                Quaternion rotation = Quaternion.LookRotation(direction);
                magePrefab.transform.rotation = rotation;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.player))
        {
            if (magePrefab != null && !magePrefab.activeSelf)
            {
                int rndTrackPoint = Random.Range(0, magePillarsTrackPoints.Length);
                magePrefab.transform.position = magePillarsTrackPoints[rndTrackPoint].position;
                mageParticle.SetActive(true);
                Invoke("InstanceMage", 1f);
            }
        }
    }

    private void InstanceMage()
    {
        magePrefab.SetActive(true);
        audioSource.Play();
    }
}
