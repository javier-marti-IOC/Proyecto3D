using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Rendering.Universal.Internal;

public class RayoController : MonoBehaviour
{
    [Header("Configuración del rayo")]
    public LineRenderer line;
    public int numPoints = 10;
    public float dispersion = 0.3f;
    public float updateFrequency = 0.02f;

    private float updateTimer = 0f;
    private bool isActive = false;
    private Vector3 startPoint;
    private Vector3 endPoint;

    public void PlayLightning(Vector3 start, Vector3 end, float duration)
    {
        startPoint = start;
        endPoint = end;

        isActive = true;
        line.enabled = true;

        // Forzar primera actualización inmediata
        UpdateLightning();
        updateTimer = 0f;

        Invoke(nameof(StopLightning), duration);
    }

    private void Update()
    {
        if (!isActive) return;

        updateTimer += Time.deltaTime;
        if (updateTimer >= updateFrequency)
        {
            UpdateLightning();
            updateTimer = 0f;
        }
    }

    private void UpdateLightning()
    {
        List<Vector3> points = new List<Vector3>();

        for (int i = 0; i < numPoints; i++)
        {
            float t = (float)i / (numPoints - 1);
            Vector3 point = Vector3.Lerp(startPoint, endPoint, t);
            point += Random.insideUnitSphere * dispersion;
            points.Add(point);
        }

        line.positionCount = points.Count;
        line.SetPositions(points.ToArray());
    }

    private void StopLightning()
    {
        isActive = false;
        line.enabled = false;
    }
}
