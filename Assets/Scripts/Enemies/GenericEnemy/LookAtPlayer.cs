using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    [SerializeField] private float rayDistance;
    private Vector3 forward;
    public Color color;
    [SerializeField] private GameObject player;

    void Awake()
    {
        player = GameObject.FindWithTag(Constants.player);
    }

    // Update is called once per frame
    void Update()
    {
        forward = transform.forward; // Obtiene la direccion hacia donde mira el enemigo en cada frame
        Utils.RotatePositionToTarget(gameObject.transform, player.transform, 15f);
    }

    public bool CentralRay()
    {
        // Raycast central
        if (Physics.Raycast(transform.position, forward, out RaycastHit hitCenter, rayDistance))
        {
            Debug.DrawRay(transform.position, forward * rayDistance, color);
            if (hitCenter.collider.CompareTag(Constants.player))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }


    }
}
