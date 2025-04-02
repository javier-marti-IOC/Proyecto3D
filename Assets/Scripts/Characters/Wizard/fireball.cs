using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireball : basicSpellObject
{

    protected override private void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);     
    }
}
