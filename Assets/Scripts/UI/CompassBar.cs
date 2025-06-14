using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassBar : MonoBehaviour
{
    //Referència a la barra
    public RectTransform compassBarTransform;

    //Símbols
    [Header("SafeZone")]
    public RectTransform safeZoneMarkerTransform;

    [Header("EarthTower")]
    public RectTransform earthTowerMarkerTransform;
    public RectTransform earthTowerDestroyedMarkerTransform;

    [Header("WaterTower")]
    public RectTransform waterTowerMarkerTransform;
    public RectTransform waterTowerDestroyedMarkerTransform;

    [Header("FireTower")]
    public RectTransform fireTowerMarkerTransform;
    public RectTransform fireTowerDestroyedMarkerTransform;

    [Header("ElectricTower")]
    public RectTransform electricTowerMarkerTransform;
    public RectTransform electricTowerDestroyedMarkerTransform;

    //Punts cardinals
    // [Header("Cardinal Points")]
    // public RectTransform northMarkerTransform;
    // public RectTransform southMarkerTransform;
    // public RectTransform eastMarkerTransform;
    // public RectTransform westMarkerTransform;

    //MainCamera
    private Transform cameraObjectTransform;

    //Objectes fisics de l'escenari
    [Header("Physic Object Scenary")]
    public Transform safeZoneObjectTransform;
    public Transform earthTowerObjectTransform;
    public Transform waterTowerObjectTransform;
    public Transform fireTowerObjectTransform;
    public Transform electricTowerObjectTransform;

    private ProgressManager pm;
    void Start()
    {
        cameraObjectTransform = Camera.main.transform;
        pm = FindObjectOfType<ProgressManager>();
    }
    void Update()
    {
        //Posicions dels marcadors a la barra respecte els objectes fisics
        SetMarkerPosition(safeZoneMarkerTransform, safeZoneObjectTransform.position);

        if (pm.IsEarthTowerDestroyed())
        {
            earthTowerDestroyedMarkerTransform.gameObject.SetActive(true);
            earthTowerMarkerTransform.gameObject.SetActive(false);
            SetMarkerPosition(earthTowerDestroyedMarkerTransform, earthTowerObjectTransform.position);
        }
        else if (!pm.IsEarthTowerDestroyed())
        {
            earthTowerDestroyedMarkerTransform.gameObject.SetActive(false);
            earthTowerMarkerTransform.gameObject.SetActive(true);
            SetMarkerPosition(earthTowerMarkerTransform, earthTowerObjectTransform.position);
        }

        if (pm.IsWaterTowerDestroyed())
        {
            waterTowerDestroyedMarkerTransform.gameObject.SetActive(true);
            waterTowerMarkerTransform.gameObject.SetActive(false);
            SetMarkerPosition(waterTowerDestroyedMarkerTransform, waterTowerObjectTransform.position);
        }
        else if (!pm.IsWaterTowerDestroyed())
        {
            waterTowerDestroyedMarkerTransform.gameObject.SetActive(false);
            waterTowerMarkerTransform.gameObject.SetActive(true);
            SetMarkerPosition(waterTowerMarkerTransform, waterTowerObjectTransform.position);
        }

        if (pm.IsFireTowerDestroyed())
        {
            fireTowerDestroyedMarkerTransform.gameObject.SetActive(true);
            fireTowerMarkerTransform.gameObject.SetActive(false);
            SetMarkerPosition(fireTowerDestroyedMarkerTransform, fireTowerObjectTransform.position);
        }
        else if (!pm.IsFireTowerDestroyed())
        {
            fireTowerDestroyedMarkerTransform.gameObject.SetActive(false);
            fireTowerMarkerTransform.gameObject.SetActive(true);
            SetMarkerPosition(fireTowerMarkerTransform, fireTowerObjectTransform.position);
        }

        if (pm.IsElectricTowerDestroyed())
        {
            electricTowerDestroyedMarkerTransform.gameObject.SetActive(true);
            electricTowerMarkerTransform.gameObject.SetActive(false);
            SetMarkerPosition(electricTowerDestroyedMarkerTransform, electricTowerObjectTransform.position);
        }
        else if (!pm.IsElectricTowerDestroyed())
        {
            electricTowerDestroyedMarkerTransform.gameObject.SetActive(false);
            electricTowerMarkerTransform.gameObject.SetActive(true);
            SetMarkerPosition(electricTowerMarkerTransform, electricTowerObjectTransform.position);
        }

        //Posicions fixes dels punts cardinals
        // SetMarkerPosition(northMarkerTransform, cameraObjectTransform.position + Vector3.forward * 1000);
        // SetMarkerPosition(southMarkerTransform, cameraObjectTransform.position + Vector3.back * 1000);
        // SetMarkerPosition(eastMarkerTransform, cameraObjectTransform.position + Vector3.right * 1000);
        // SetMarkerPosition(westMarkerTransform, cameraObjectTransform.position + Vector3.left * 1000);
    }

    private void SetMarkerPosition(RectTransform markerTransform, Vector3 worldPosition)
    {
        //Vector desde la càmera fins a l'objectiu
        Vector3 directionToTarget = worldPosition - cameraObjectTransform.position;

        //Angle entre la direcció del jugador i l'objectiu (pla horitzontal)
        float signedAngle = Vector3.SignedAngle(new Vector3(cameraObjectTransform.forward.x, 0, cameraObjectTransform.forward.z),
            new Vector3(directionToTarget.x, 0, directionToTarget.z),
            Vector3.up
        );

        //Converteix l'angle en una posició apte pel rectangle de la brúixola i passa a un valor entre -0.5 i 0.5
        float compassPosition = Mathf.Clamp(signedAngle / Camera.main.fieldOfView, -0.5f, 0.5f);

        //Coloca l'icono en el rectangle depenent dela posició
        markerTransform.anchoredPosition = new Vector2(compassBarTransform.rect.width * compassPosition, 0);

        // Escaladt per distancia i limitar-lo entre 0.5 i 1
        float distance = Vector3.Distance(cameraObjectTransform.position, worldPosition);
        float scaleFactor = Mathf.Clamp(1 / (distance * 0.02f), 0.85f, 1f);
        markerTransform.localScale = new Vector3(1f * scaleFactor, 1f * scaleFactor, 1f);
    }
}
