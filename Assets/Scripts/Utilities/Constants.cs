using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants : MonoBehaviour
{
    public const string player = "Player";
    public const string navMeshSurface = "Plane";
    public const string walkable = "Walkable";
    public const string water = "Water";
    public const string fire = "Fire";
    public const string earth = "Earth";
    public const string electric = "Electric";

    
}

public enum Element
    {
        None,
        Earth,
        Water,
        Fire,
        Electric
    };
