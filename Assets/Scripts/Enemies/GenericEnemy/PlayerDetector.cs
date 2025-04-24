using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    [SerializeField] private float rayDistance = 10f;
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
        RaycastHit hitCenter;
        if (Physics.Raycast(transform.position, forward, out hitCenter, rayDistance))
        {
            if (hitCenter.collider.CompareTag(Constants.player))
            {
                enemy.playerDetected = true;
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
        RaycastHit hitLeft;
        if (Physics.Raycast(transform.position, leftDir, out hitLeft, rayDistance))
        {
            if (hitLeft.collider.CompareTag(Constants.player))
            {
                enemy.playerDetected = true;
            }

        }

        // Raycast izquierdo pequeño
        RaycastHit hitSoftLeft;
        if (Physics.Raycast(transform.position, leftSoftDir, out hitSoftLeft, rayDistance))
        {
            if (hitSoftLeft.collider.CompareTag(Constants.player))
            {
                enemy.playerDetected = true;
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
        RaycastHit hitRight;
        if (Physics.Raycast(transform.position, rightDir, out hitRight, rayDistance))
        {
            if (hitRight.collider.CompareTag(Constants.player))
            {
                enemy.playerDetected = true;
            }
        }
        // Raycast derecho pequeño
        RaycastHit hitSoftRight;
        if (Physics.Raycast(transform.position, rightSoftDir, out hitSoftRight, rayDistance))
        {
            if (hitSoftRight.collider.CompareTag(Constants.player))
            {
                enemy.playerDetected = true;
            }
        }

        Debug.DrawRay(transform.position, rightDir * rayDistance, Color.blue);
        Debug.DrawRay(transform.position, rightSoftDir * rayDistance, Color.blue);

    }
}
