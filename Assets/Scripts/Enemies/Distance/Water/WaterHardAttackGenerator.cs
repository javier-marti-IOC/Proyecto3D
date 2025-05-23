using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterHardAttackGenerator : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform middleHands;
    public AudioSource waterHeavyAttack;

    public void ShootBullets()
    {
        float[] angles = { 0f, 15f, -15f, 30f, -30f };
        float separation = 0.25f; // separación lateral entre balas (ajusta a tu gusto)

        // Base forward horizontal
        Vector3 flatForward = GetComponent<Enemy>().transform.forward;
        flatForward.y = 0f;
        flatForward.Normalize();
        waterHeavyAttack.Play();
        
        for (int i = 0; i < angles.Length; i++)
        {
            float angle = angles[i];

            // Dirección rotada en el plano horizontal
            Vector3 rotatedDirection = Quaternion.AngleAxis(angle, Vector3.up) * flatForward;
            Quaternion bulletRotation = Quaternion.LookRotation(rotatedDirection);

            // Lateral offset relativo al right plano también
            Vector3 flatRight = Vector3.Cross(Vector3.up, rotatedDirection).normalized;

            // Calcula un offset lateral (en el eje local X de middleHands)
            // La bala central no se desplaza, las demás si
            float lateralOffset = 0f;
            if (i == 1) lateralOffset = separation;       // 15°
            else if (i == 2) lateralOffset = -separation; // -15°
            else if (i == 3) lateralOffset = separation * 2;    // 30°
            else if (i == 4) lateralOffset = -separation * 2;   // -30°

            // Calcula la posicion inicial sumando el offset lateral
            Vector3 spawnPos = middleHands.position + flatRight * lateralOffset;

            // Instanciar la bala con la nueva rotacion
            GameObject spawnedBullet = Instantiate(bulletPrefab, spawnPos, bulletRotation);
            HardBullet bullet = spawnedBullet.GetComponent<HardBullet>();
            bullet.GetComponent<HardBullet>().enemy = GetComponent<Enemy>();
            bullet.enemy = GetComponent<Enemy>();


        }
    }
}
