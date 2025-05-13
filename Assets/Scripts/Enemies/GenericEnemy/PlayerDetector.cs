using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    [SerializeField] private float rayDistance;
    private Vector3 forward;
    private Enemy enemy;

    void Awake()
    {
        enemy = gameObject.GetComponentInParent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        forward = transform.forward; // Obtiene la direccion hacia donde mira el enemigo en cada frame

        CentralRay();
        LeftRays();
        RightRays();

    }

    private void CentralRay()
    {

        // Raycast central
        if (Physics.Raycast(transform.position, forward, out RaycastHit hitCenter, rayDistance))
        {
            if (hitCenter.collider.CompareTag(Constants.player))
            {
                enemy.PlayerDetected();
            }
        }

        Debug.DrawRay(transform.position, forward * rayDistance, Color.red);

    }

    private void LeftRays()
    {

        Quaternion leftRotation = Quaternion.Euler(0, -30, 0);
        Quaternion leftSoftRotation = Quaternion.Euler(0, -15, 0);

        Vector3 leftDir = leftRotation * forward;
        Vector3 leftSoftDir = leftSoftRotation * forward;

        // Raycast izquierdo
        if (Physics.Raycast(transform.position, leftDir, out RaycastHit hitLeft, rayDistance))
        {
            if (hitLeft.collider.CompareTag(Constants.player))
            {
                enemy.PlayerDetected();
            }

        }

        // Raycast izquierdo pequeño
        if (Physics.Raycast(transform.position, leftSoftDir, out RaycastHit hitSoftLeft, rayDistance))
        {
            if (hitSoftLeft.collider.CompareTag(Constants.player))
            {
                enemy.PlayerDetected();
            }

        }


        Debug.DrawRay(transform.position, leftDir * rayDistance, Color.green);
        Debug.DrawRay(transform.position, leftSoftDir * rayDistance, Color.green);



    }

    private void RightRays()
    {

        Quaternion rightRotation = Quaternion.Euler(0, 30, 0);
        Quaternion rightSoftRotation = Quaternion.Euler(0, 15, 0);

        Vector3 rightDir = rightRotation * forward;
        Vector3 rightSoftDir = rightSoftRotation * forward;

        // Raycast derecho
        if (Physics.Raycast(transform.position, rightDir, out RaycastHit hitRight, rayDistance))
        {
            if (hitRight.collider.CompareTag(Constants.player))
            {
                enemy.PlayerDetected();
            }
        }
        // Raycast derecho pequeño
        if (Physics.Raycast(transform.position, rightSoftDir, out RaycastHit hitSoftRight, rayDistance))
        {
            if (hitSoftRight.collider.CompareTag(Constants.player))
            {
                enemy.PlayerDetected();
            }
        }

        Debug.DrawRay(transform.position, rightDir * rayDistance, Color.blue);
        Debug.DrawRay(transform.position, rightSoftDir * rayDistance, Color.blue);

    }
}
