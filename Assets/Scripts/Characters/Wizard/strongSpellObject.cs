using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class strongSpellObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log(collider.gameObject.tag);
        if (collider.gameObject.tag.Equals("Player"))
        {
            collider.gameObject.GetComponent<ThirdPersonController>().MoveSpeed = 1;
            collider.gameObject.GetComponent<ThirdPersonController>().SprintSpeed = 2.5f;
        }       
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag.Equals("Player"))
        {
            collider.gameObject.GetComponent<ThirdPersonController>().MoveSpeed = 2;
            collider.gameObject.GetComponent<ThirdPersonController>().SprintSpeed = 5f;
        }       
    }
    
}
