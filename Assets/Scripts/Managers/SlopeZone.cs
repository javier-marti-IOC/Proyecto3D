using UnityEngine;

public class SlopeZone : MonoBehaviour
{
    [Tooltip("Nuevo slope limit mientras el jugador esté dentro de esta zona")]
    public float zoneSlopeLimit = 55f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        var controller = other.GetComponent<CharacterSlopeHandler>();
        if (controller != null)
        {
            controller.EnterSlopeZone(zoneSlopeLimit);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        var controller = other.GetComponent<CharacterSlopeHandler>();
        if (controller != null)
        {
            controller.ExitSlopeZone();
        }
    }
}
