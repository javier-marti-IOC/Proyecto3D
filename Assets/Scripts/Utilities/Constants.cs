using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants : MonoBehaviour
{
    public const string player = "Player";
    public const string walkable = "Walkable";
    public const string water = "Water";
    public const string fire = "Fire";
    public const string earth = "Earth";
    public const string electric = "Electric";
    public const string pointPatrol = "PointPatrol";
    public const string enemy = "Enemy";
    public const string state = "State";
    public const string tower = "Tower";
    public const string waterBullet = "WaterBullet";
    public const string speed = "Speed";
    public const string drop = "Drop";
    public const string sword = "Sword";
}

public enum Element
    {
        None,
        Earth,
        Water,
        Fire,
        Electric
    };
