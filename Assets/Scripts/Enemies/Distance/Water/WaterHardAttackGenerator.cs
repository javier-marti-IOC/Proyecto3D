using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterHardAttackGenerator : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform middleHands;

    public void ShootBullets()
    {
        float[] angles = { 0f, 15f, -15f, 30f, -30f };
        float separation = 0.25f; // separación lateral entre balas (ajusta a tu gusto)

        for (int i = 0; i < angles.Length; i++)
        {
            float angle = angles[i];
            // Crear la rotacion en la direccion deseada
            Quaternion bulletRotation = Quaternion.Euler(0f, angle, 0f);

            // Calcula un offset lateral (en el eje local X de middleHands)
            // La bala central no se desplaza, las demás si
            float lateralOffset = 0f;
            if (i == 1) lateralOffset = separation;       // 15°
            else if (i == 2) lateralOffset = -separation; // -15°
            else if (i == 3) lateralOffset = separation * 2;    // 30°
            else if (i == 4) lateralOffset = -separation * 2;   // -30°

            // Calcula la posicion inicial sumando el offset lateral
            Vector3 spawnPos = middleHands.position + middleHands.right * lateralOffset;

            // Instanciar la bala con la nueva rotacion
            GameObject spawnedBullet = Instantiate(bulletPrefab, spawnPos, bulletRotation);
            HardBullet bullet = spawnedBullet.GetComponent<HardBullet>();
            bullet.GetComponent<HardBullet>().enemy = GetComponent<Enemy>();

        }
    }
}
