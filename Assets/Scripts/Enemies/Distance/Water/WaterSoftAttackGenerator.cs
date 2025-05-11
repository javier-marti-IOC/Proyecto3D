using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSoftAttackGenerator : MonoBehaviour
{
    public GameObject bullet;
    public Transform rightHand;

    public void ShootBullet(){
        Instantiate(bullet , rightHand.transform.position, Quaternion.identity);
    }
}
