using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSoftAttackGenerator : MonoBehaviour
{
    public GameObject bullet;
    public Transform rightHand;

    public void ShootBullet(){
        // Instanciar la bala y guardar la referencia
        GameObject spawnedBullet = Instantiate(bullet, rightHand.position, Quaternion.identity);

        // Obtener el componente Enemy de este mismo objeto (enemigo que dispara) 
        spawnedBullet.GetComponent<SoftBullet>().enemy = GetComponent<Enemy>();
    }
}
