using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GrassSpawnerV2 : EditorWindow
{
    public GameObject parentObject;
    public Material targetMaterial;
    public List<GameObject> grassPrefabs = new List<GameObject>();
    public float raycastHeight = 10f;
    public float spacing = 1f;

    private List<GameObject> spawnedGrass = new List<GameObject>();

    [MenuItem("Tools/Grass Spawner V2")]
    public static void ShowWindow()
    {
        GetWindow<GrassSpawnerV2>("Grass Spawner V2");
    }

    private void OnGUI()
    {
        GUILayout.Label("Grass Spawner Settings", EditorStyles.boldLabel);

        parentObject = (GameObject)EditorGUILayout.ObjectField("Parent Object", parentObject, typeof(GameObject), true);
        targetMaterial = (Material)EditorGUILayout.ObjectField("Target Material", targetMaterial, typeof(Material), false);

        SerializedObject so = new SerializedObject(this);
        SerializedProperty prefabsProp = so.FindProperty("grassPrefabs");
        EditorGUILayout.PropertyField(prefabsProp, true);
        so.ApplyModifiedProperties();

        raycastHeight = EditorGUILayout.FloatField("Raycast Height", raycastHeight);
        spacing = EditorGUILayout.FloatField("Spacing", spacing);

        if (GUILayout.Button("Spawn Grass"))
        {
            SpawnGrass();
        }

        if (GUILayout.Button("Clear Spawned Grass"))
        {
            ClearGrass();
        }
    }

    private void SpawnGrass()
    {
        if (parentObject == null || targetMaterial == null || grassPrefabs.Count == 0)
        {
            Debug.LogWarning("Missing parameters! Make sure Parent Object, Target Material, and Grass Prefabs are set.");
            return;
        }

        MeshRenderer[] meshRenderers = parentObject.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in meshRenderers)
        {
            if (renderer.sharedMaterial != targetMaterial)
                continue;

            Bounds bounds = renderer.bounds;

            for (float x = bounds.min.x; x < bounds.max.x; x += spacing)
            {
                for (float z = bounds.min.z; z < bounds.max.z; z += spacing)
                {
                    Vector3 rayOrigin = new Vector3(x, bounds.max.y + raycastHeight, z);
                    if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, raycastHeight * 2f))
                    {
                        if (hit.collider.GetComponent<MeshRenderer>() == renderer)
                        {
                            GameObject prefab = grassPrefabs[Random.Range(0, grassPrefabs.Count)];
                            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                            instance.transform.position = hit.point;
                            instance.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                            instance.transform.SetParent(parentObject.transform);
                            spawnedGrass.Add(instance);
                        }
                    }
                }
            }
        }
    }

    private void ClearGrass()
    {
        foreach (var grass in spawnedGrass)
        {
            if (grass != null)
                DestroyImmediate(grass);
        }
        spawnedGrass.Clear();
    }
}
