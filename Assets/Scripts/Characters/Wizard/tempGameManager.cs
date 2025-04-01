using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempGameManager : MonoBehaviour
{
    [HideInInspector]
    public enum Element
    {
        None,
        Grass,
        Water,
        Fire,
        Electric
    };
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static bool PositiveElementInteraction(Element element1,Element element2)
    {
        if (element1 == Element.Grass && element2 == Element.Electric)
        {
            return true;
        }
        else if (element1 == Element.Water && element2 == Element.Fire)
        {
            return true;
        }
        else if (element1 == Element.Fire && element2 == Element.Grass)
        {
            return true;
        }
        else if (element1 == Element.Electric && element2 == Element.Water)
        {
            return true;
        }
        return false;
    }

    public static Element getCounterElement(Element element)
    {
        if (element == Element.None)
        {
            int num = Random.Range(1,5);
            if (num == 1) return Element.Fire;
            else if (num == 2) return Element.Electric;
            else if (num == 3) return Element.Water;
            else return Element.Grass;
        } 
        else if (element == Element.Grass)
        {
            return Element.Fire;
        }
        else if (element == Element.Water)
        {
            return Element.Electric;
        }
        else if (element == Element.Fire)
        {
            return Element.Water;
        }
        else
        {
            return Element.Grass;
        }
    }
}
