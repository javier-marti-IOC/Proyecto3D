using UnityEngine;

public class CharacterSlopeHandler : MonoBehaviour
{
    public float defaultSlopeLimit = 45f;

    private CharacterController cc;
    private int activeZones = 0;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
        if (cc == null)
            Debug.LogError("No hay CharacterController en este GameObject.");
    }

    public void EnterSlopeZone(float newLimit)
    {
        activeZones++;
        cc.slopeLimit = newLimit;
    }

    public void ExitSlopeZone()
    {
        activeZones = Mathf.Max(0, activeZones - 1);
        if (activeZones == 0)
            cc.slopeLimit = defaultSlopeLimit;
    }
}
