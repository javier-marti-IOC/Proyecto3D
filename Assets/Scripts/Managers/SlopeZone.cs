using UnityEngine;

public class SlopeZone : MonoBehaviour
{
    [Tooltip("Nuevo slope limit mientras el jugador esté dentro de esta zona")]
    private bool isFirstTimeEnter;
    private bool isFirstTimeExit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.player) && isFirstTimeEnter)
        {
            isFirstTimeEnter = false;
            isFirstTimeExit = true;
            other.GetComponent<CharacterController>().slopeLimit = 55;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Constants.player) && isFirstTimeExit)
        {
            isFirstTimeEnter = true;
            isFirstTimeExit = false;
            other.GetComponent<CharacterController>().slopeLimit = 55;
        }
    }
}
