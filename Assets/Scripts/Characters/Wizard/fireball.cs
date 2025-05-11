using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : BasicSpellObject
{

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);     
    }
}
