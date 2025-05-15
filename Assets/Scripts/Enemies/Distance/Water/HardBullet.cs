using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardBullet : MonoBehaviour
{
    public Enemy enemy;
    private Rigidbody spellRB;
    public float speed;

    void Start()
    {
        spellRB = gameObject.GetComponent<Rigidbody>();
        spellRB.velocity = transform.forward * speed;
        Destroy(gameObject, 2.8f); // Autodestruir después de X segundos
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        //Esto se eliminara en un futuro
        if (collision.gameObject.CompareTag(Constants.player))
        {
            if (enemy != null)
            {
                enemy.GetComponent<DistanceBT>().PlayerHitted();
            }
        }
        else if (collision.gameObject.CompareTag(Constants.waterBullet) || collision.gameObject.CompareTag(Constants.enemy))
        {
            return;
        }
        Destroy(gameObject);
    }
}
