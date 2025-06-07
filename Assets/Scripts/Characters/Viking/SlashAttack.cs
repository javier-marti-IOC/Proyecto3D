using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashAttack : MonoBehaviour
{
    private VikingController vikingController;
    private Rigidbody rb;
    private float lifeTime = 0.5f;
    private Element element;


    void Start()
    {
        vikingController = FindObjectOfType<VikingController>();
        rb = GetComponent<Rigidbody>();
        rb.rotation = vikingController.transform.rotation;
        element = vikingController.activeElement;
    }
    void Update()
    {
        rb.velocity = vikingController.transform.forward * 30f;
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0)
        {
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        vikingController.SlashAttackEnter(other, element);
    }
}
