using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drops : MonoBehaviour
{
    public Element element;
    private int min = 40;
    private int max = 60;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.player))
        {
            if (element == Element.None)
            {
                other.GetComponent<VikingController>().earthMana = 100;
                other.GetComponent<VikingController>().waterMana = 100;
                other.GetComponent<VikingController>().fireMana = 100;
                other.GetComponent<VikingController>().electricMana = 100;
            }
            else if (element == Element.Earth)
            {
                other.GetComponent<VikingController>().earthMana += Random.Range(min,max);
                if (other.GetComponent<VikingController>().earthMana > 100) other.GetComponent<VikingController>().earthMana = 100;
            }
            else if (element == Element.Water)
            {
                other.GetComponent<VikingController>().waterMana += Random.Range(min,max);
                if (other.GetComponent<VikingController>().waterMana > 100) other.GetComponent<VikingController>().waterMana = 100;
            }   
            else if (element == Element.Fire)
            {
                other.GetComponent<VikingController>().fireMana += Random.Range(min,max);
                if (other.GetComponent<VikingController>().fireMana > 100) other.GetComponent<VikingController>().fireMana = 100;
            }   
            else if (element == Element.Electric)
            {
                other.GetComponent<VikingController>().electricMana += Random.Range(min,max);
                if (other.GetComponent<VikingController>().electricMana > 100) other.GetComponent<VikingController>().electricMana = 100;
            }      
            Destroy(gameObject);
        }
    }
}
