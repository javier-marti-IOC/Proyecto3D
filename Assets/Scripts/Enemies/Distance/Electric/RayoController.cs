using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Rendering.Universal.Internal;

public class RayoController : MonoBehaviour
{
    public GameObject impactPosition;
    public int numPoints;
    public float dispersion;
    public float frequency;

    private LineRenderer line;
    private float time = 0;

    void Start()
    {
        line = GetLine();
        GameObject player = GameObject.FindWithTag("Player");
        Transform spine1 = player.GetComponentsInChildren<Transform>().FirstOrDefault(t => t.name == "mixamorig:Spine2");
        impactPosition = spine1.gameObject;
    }

    void Update()
    {
        time += Time.deltaTime;

        if (time > frequency)
        {
            UpdatePoints(this.line);
            time = 0;
        }
    }

    private LineRenderer GetLine()
    {
        return GetComponent<LineRenderer>();
    }

    private void UpdatePoints(LineRenderer line)
    {
        List<Vector3> points = InterpolatePoints(Vector3.zero, impactPosition.transform.localPosition, numPoints);
        line.positionCount = points.Count;
        line.SetPositions(points.ToArray());
    }

    private List<Vector3> InterpolatePoints(Vector3 start, Vector3 impactPosition, int totalPoints)
    {
        List<Vector3> list = new List<Vector3>();

        for (int i = 0; i < totalPoints; i++)
        {
            list.Add(Vector3.Lerp(start, impactPosition, (float)i / totalPoints) + RandomPoints());
        }

        return list;
    }

    private Vector3 RandomPoints()
    {
        return Random.insideUnitSphere.normalized * Random.Range(0, dispersion);
    }
}
