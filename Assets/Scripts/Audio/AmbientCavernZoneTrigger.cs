using UnityEngine;

public class AmbientCavernZoneTrigger : MonoBehaviour
{
    [Header("Collider principal del jugador")]
    public Collider playerCollider;  // arrástralo desde el inspector

    private bool isPlayerInside = false;
    void Start()
    {
        AudioManager.Instance.Play("AmbienceForest");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (Time.deltaTime == 0f) return;
        if (other == playerCollider && !isPlayerInside)
        {
            Debug.Log("OnTriggerEnter Cavern");
            isPlayerInside = true;
            AudioManager.Instance.Stop("AmbienceForest");
            AudioManager.Instance.Play("AmbienceCavern");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (Time.deltaTime == 0f) return;
        if (other == playerCollider && isPlayerInside)
        {
            Debug.Log("OnTriggerExit Cavern");
            isPlayerInside = false;
            AudioManager.Instance.Stop("AmbienceCavern");
            AudioManager.Instance.Play("AmbienceForest");
        }
    }
}