using System;
using UnityEngine;
using UnityEngine.AI;

public class MoveToClick : MonoBehaviour
{
    public Camera mainCamera;
    public NavMeshAgent navMeshAgent;

    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Move();   
        }
    }

    void Move()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            navMeshAgent.SetDestination(hit.point);
        }
    }
}
