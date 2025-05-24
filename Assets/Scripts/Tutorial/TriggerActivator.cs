using UnityEngine;

public class TriggerActivator : MonoBehaviour
{
    [Tooltip("Objetos que se activarán inmediatamente al entrar el jugador.")]
    public GameObject[] objectsToActivate;

    [Tooltip("Este objeto se activará 1 segundo después.")]
    public GameObject delayedObject;

    [Tooltip("Tag del jugador.")]
    public string playerTag = "Player";

    private bool alreadyActivated = false;

    [Header("Progress Manager")]
    public ProgressManager progressManager;
    public ProgressData progressData;
    public GameObject rocksFallingWall;
    public AudioSource rocksFallingSound;

    void Start()
    {
        if (ProgressManager.Instance.Data.tutorial == true)
        {
            if (rocksFallingWall != null && !rocksFallingWall.activeSelf)
            {
                rocksFallingWall.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (alreadyActivated || !other.CompareTag(playerTag) || rocksFallingWall.activeSelf) return;

        alreadyActivated = true;
        rocksFallingSound.Play();
        // Activar los objetos inmediatos
        foreach (GameObject obj in objectsToActivate)
        {
            if (obj != null)
                obj.SetActive(true);
        }

        // Activar el objeto con retraso
        if (delayedObject != null)
        {
            Invoke(nameof(ActivateDelayedObject), 1f);
        }

        // Destruir el objeto que contiene este script y el trigger
        Destroy(gameObject, 1.1f); // Esperamos un poco más para que no se destruya antes de activar el objeto retrasado
    }

    private void ActivateDelayedObject()
    {
        delayedObject.SetActive(true);
    }
}
