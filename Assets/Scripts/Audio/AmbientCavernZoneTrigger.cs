using UnityEngine;

public class AmbientCavernZoneTrigger : MonoBehaviour
{
    private bool isPlayerInside = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            AudioManager.Instance.Stop("AmbienceForest");
            AudioManager.Instance.Play("AmbienceCavern");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isPlayerInside)
        {
            isPlayerInside = false;
            AudioManager.Instance.Stop("AmbienceCavern");
            AudioManager.Instance.Play("AmbienceForest");
        }
    }
}