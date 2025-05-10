using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    void LateUpdate()
    {
        if (Camera.main != null)
        {
            transform.LookAt(Camera.main.transform);
            //transform.Rotate(0, 180, 0); // Esto lo voltea si se ve al revés
        }
    }
}