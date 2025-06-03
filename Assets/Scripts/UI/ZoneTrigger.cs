using UnityEngine;

public class ZoneTrigger : MonoBehaviour
{
    public ZonePanel zonePanel; // Asigna esto desde el Inspector
    public string introductoryText;
    public string zoneName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.player))
        {
            zonePanel.ShowPanel(introductoryText, zoneName);
        }
    }
}
