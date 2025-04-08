using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempGameManager : MonoBehaviour
{

    public static bool PositiveElementInteraction(Element element1,Element element2)
    {
        if (element1 == Element.Earth && element2 == Element.Electric)
        {
            return true;
        }
        else if (element1 == Element.Water && element2 == Element.Fire)
        {
            return true;
        }
        else if (element1 == Element.Fire && element2 == Element.Earth)
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
            else return Element.Earth;
        } 
        else if (element == Element.Earth)
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
            return Element.Earth;
        }
    }
}
