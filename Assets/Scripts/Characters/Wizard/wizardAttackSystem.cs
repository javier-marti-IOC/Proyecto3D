using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wizardAttackSystem : MonoBehaviour
{
    public GameObject basicSpell;
    public Transform spellExitPoint;
    private float t = 2; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((t -= Time.deltaTime) <= 0) 
        { 
            basicSpellCast();
            t = 2; 
        }    
    }

    void basicSpellCast()
    {  
        Instantiate(basicSpell, spellExitPoint.position, Quaternion.identity);
    }
}
