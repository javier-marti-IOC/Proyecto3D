using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicSpellObject : MonoBehaviour
{
    private GameObject target;
    private Rigidbody spellRB;
    public float fallValue;
    public float speed;
    public float rotationSpeed = 5f;

    void Start()
    {
        target = GameObject.Find("PlayerAgent");
        spellRB = gameObject.GetComponent<Rigidbody>();
        transform.LookAt(target.transform);
    }
    
    void FixedUpdate() 
    {
        if (target == null) return;

        Vector3 direction = (new Vector3(target.transform.position.x, transform.position.y - fallValue, target.transform.position.z) - transform.position).normalized;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

        spellRB.velocity = transform.forward * speed; 
    }

    protected private virtual void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);       
    }
}
