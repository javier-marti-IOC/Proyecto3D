using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int DamageCalulator(Element dealerElement,int dealerBasicDamage,int dealerElementalDamege,Element takerElement)
    {
        float basicDamageRange = Random.Range(dealerBasicDamage * 0.85f,dealerBasicDamage * 1.15f);
        float elementDamageRange =  Random.Range(dealerElementalDamege * 0.85f,dealerElementalDamege * 1.15f);
        switch (ElementInteraction(dealerElement,takerElement))
        {
            case 1:
                return (int)(basicDamageRange + elementDamageRange * 1.5f);
            case 0:
                return (int)(basicDamageRange + elementDamageRange);
            case -1:
                return (int)(basicDamageRange + elementDamageRange * 0.75f);
        }
        return 0;
    }
    public int ElementInteraction(Element element1,Element element2)
    {
        if (element1 == Element.Earth && element2 == Element.Electric)
        {
            return 1;
        }
        else if (element1 == Element.Water && element2 == Element.Fire)
        {
            return 1;
        }
        else if (element1 == Element.Fire && element2 == Element.Earth)
        {
            return 1;
        }
        else if (element1 == Element.Electric && element2 == Element.Water)
        {
            return 1;
        }
        else if (element1 == Element.Earth && element2 == Element.Fire)
        {
            return -1;
        }
        else if (element1 == Element.Water && element2 == Element.Electric)
        {
            return -1;
        }
        else if (element1 == Element.Fire && element2 == Element.Water)
        {
            return -1;
        }
        else if (element1 == Element.Electric && element2 == Element.Earth)
        {
            return -1;
        }
        return 0;
    }

    public Element getCounterElement(Element element)
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
