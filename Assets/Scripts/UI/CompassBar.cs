using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassBar : MonoBehaviour
{
    //Referència a la barra
    public RectTransform compassBarTransform;

    //Símbols
    public RectTransform safeZoneMarkerTransform;
    public RectTransform earthTowerMarkerTransform;
    public RectTransform waterTowerMarkerTransform;
    public RectTransform fireTowerMarkerTransform;
    public RectTransform electricTowerMarkerTransform;
    
    //Punts cardinals
    public RectTransform northMarkerTransform;
    public RectTransform southMarkerTransform;
    public RectTransform eastMarkerTransform;
    public RectTransform westMarkerTransform;

    //MainCamera
    private Transform cameraObjectTransform;

    //Objectes fisics de l'escenari
    public Transform safeZoneObjectTransform;
    public Transform earthTowerObjectTransform;
    public Transform waterTowerObjectTransform;
    public Transform fireTowerObjectTransform;
    public Transform electricTowerObjectTransform;
    void Start()
    {
        cameraObjectTransform = Camera.main.transform;
    }
    void Update()
    {
        //Posicions dels marcadors a la barra respecte els objectes fisics
        SetMarkerPosition(safeZoneMarkerTransform, safeZoneObjectTransform.position);
        SetMarkerPosition(earthTowerMarkerTransform, earthTowerObjectTransform.position);
        SetMarkerPosition(waterTowerMarkerTransform, waterTowerObjectTransform.position);
        SetMarkerPosition(fireTowerMarkerTransform, fireTowerObjectTransform.position);
        SetMarkerPosition(electricTowerMarkerTransform, electricTowerObjectTransform.position);

        //Posicions fixes dels punts cardinals
        SetMarkerPosition(northMarkerTransform, cameraObjectTransform.position + Vector3.forward * 1000);
        SetMarkerPosition(southMarkerTransform, cameraObjectTransform.position + Vector3.back * 1000);
        SetMarkerPosition(eastMarkerTransform, cameraObjectTransform.position + Vector3.right * 1000);
        SetMarkerPosition(westMarkerTransform, cameraObjectTransform.position + Vector3.left * 1000);
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
        float scaleFactor = Mathf.Clamp(1 / (distance * 0.02f), 0.5f, 1f); // Escala relativa entre 0.5x y 1x
        markerTransform.localScale = new Vector3(0.05470015f * scaleFactor, 1f * scaleFactor, 1f);
    }
}
