using UnityEngine;

public class SlopeZone : MonoBehaviour
{
    [Tooltip("Nuevo slope limit mientras el jugador esté dentro de esta zona")]
    public float zoneSlopeLimit = 55f;

    private bool initialized = false;

    private void OnTriggerStay(Collider other)
    {
        if (!initialized && other.CompareTag(Constants.player))
        {
            var handler = other.GetComponent<CharacterSlopeHandler>();
            if (handler != null)
            {
                handler.EnterSlopeZone(zoneSlopeLimit);
                initialized = true;
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.player))
        {
            var handler = other.GetComponent<CharacterSlopeHandler>();
            if (handler != null)
            {
                handler.EnterSlopeZone(zoneSlopeLimit);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Constants.player))
        {
            var handler = other.GetComponent<CharacterSlopeHandler>();
            if (handler != null)
            {
                handler.ExitSlopeZone();
            }
        }
    }
}
