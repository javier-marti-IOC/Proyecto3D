using UnityEngine;

public class AmbientZoneTrigger : MonoBehaviour
{
    [Tooltip("Nombre del sonido definido en el array 'ambience' del AudioManager")]
    public string ambientSoundName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.Instance?.Play(ambientSoundName);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.Instance?.Stop(ambientSoundName);
        }
    }
}