using UnityEngine;

public class SlopeZone : MonoBehaviour
{
    [Tooltip("Nuevo slope limit mientras el jugador esté dentro de esta zona")]
    public float zoneSlopeLimit = 55f;

    [Tooltip("Collider del jugador que debe activar esta zona")]
    public Collider playerCollider;

    private void OnTriggerEnter(Collider other)
    {
        if (other == playerCollider)
        {
            CharacterSlopeHandler handler = other.GetComponent<CharacterSlopeHandler>();
            if (handler == null)
                handler = other.GetComponentInParent<CharacterSlopeHandler>();

            if (handler != null)
            {
                handler.EnterSlopeZone(zoneSlopeLimit);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == playerCollider)
        {
            CharacterSlopeHandler handler = other.GetComponent<CharacterSlopeHandler>();
            if (handler == null)
                handler = other.GetComponentInParent<CharacterSlopeHandler>();

            if (handler != null)
            {
                handler.ExitSlopeZone();
            }
        }
    }
}
