using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GrassSpawnerV2 : EditorWindow
{
    [Tooltip("Parent object containing meshes with the target material.")]
    public GameObject parentObject;

    [Tooltip("Material used to identify the surface where grass should spawn.")]
    public Material targetMaterial;

    [Tooltip("List of grass prefabs to randomly spawn.")]
    public List<GameObject> grassPrefabs = new List<GameObject>();

    [Tooltip("Height above the mesh from which raycasts will be fired.")]
    public float raycastHeight = 10f;

    [Tooltip("Base spacing between each grass instance.")]
    public float spacing = 1f;

    [Tooltip("Random offset added to spacing to avoid perfect grid placement.")]
    public float randomOffset = 0.3f;

    [Tooltip("Name assigned to the parent object that groups all spawned grass.")]
    public string groupName = "GrassGroup";

    [Tooltip("File path where the group prefab will be saved.")]
    public string prefabSavePath = "Assets/GeneratedGrass.prefab";

    private GameObject currentGroup;
    private List<GameObject> spawnedGrass = new List<GameObject>();

    [MenuItem("Tools/Grass Spawner V2")]
    public static void ShowWindow()
    {
        GetWindow<GrassSpawnerV2>("Grass Spawner V2");
    }

    private void OnGUI()
    {
        GUILayout.Label("Grass Spawner Settings", EditorStyles.boldLabel);

        parentObject = (GameObject)EditorGUILayout.ObjectField(new GUIContent("Parent Object", "Parent object containing meshes with the target material."), parentObject, typeof(GameObject), true);
        targetMaterial = (Material)EditorGUILayout.ObjectField(new GUIContent("Target Material", "Material used to identify the surface where grass should spawn."), targetMaterial, typeof(Material), false);

        SerializedObject so = new SerializedObject(this);
        SerializedProperty prefabsProp = so.FindProperty("grassPrefabs");
        EditorGUILayout.PropertyField(prefabsProp, new GUIContent("Grass Prefabs", "List of grass prefabs to randomly spawn."), true);
        so.ApplyModifiedProperties();

        raycastHeight = EditorGUILayout.FloatField(new GUIContent("Raycast Height", "Height above the mesh from which raycasts will be fired."), raycastHeight);
        spacing = EditorGUILayout.FloatField(new GUIContent("Spacing", "Base spacing between each grass instance."), spacing);
        randomOffset = EditorGUILayout.Slider(new GUIContent("Random Offset", "Random offset added to spacing to avoid perfect grid placement."), randomOffset, 0f, spacing * 0.5f);

        GUILayout.Space(10);
        groupName = EditorGUILayout.TextField(new GUIContent("Group Name", "Name assigned to the parent object that groups all spawned grass."), groupName);
        prefabSavePath = EditorGUILayout.TextField(new GUIContent("Prefab Save Path", "File path where the group prefab will be saved."), prefabSavePath);

        if (GUILayout.Button("Spawn Grass"))
        {
            SpawnGrass();
        }

        if (GUILayout.Button("Clear From Selection"))
        {
            ClearFromParentOnly();
        }

        if (GUILayout.Button("Clear All Spawned Grass"))
        {
            ClearGrass();
        }

        if (GUILayout.Button("Save as Prefab"))
        {
            SaveAsPrefab();
        }
    }

    private void SpawnGrass()
    {
        if (parentObject == null || targetMaterial == null || grassPrefabs.Count == 0)
        {
            Debug.LogWarning("Missing parameters! Make sure Parent Object, Target Material, and Grass Prefabs are set.");
            return;
        }

        currentGroup = new GameObject(groupName);
        currentGroup.transform.SetParent(parentObject.transform);

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
                    float offsetX = Random.Range(-randomOffset, randomOffset);
                    float offsetZ = Random.Range(-randomOffset, randomOffset);
                    Vector3 rayOrigin = new Vector3(x + offsetX, bounds.max.y + raycastHeight, z + offsetZ);

                    if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, raycastHeight * 2f))
                    {
                        if (hit.collider.GetComponent<MeshRenderer>() == renderer)
                        {
                            GameObject prefab = grassPrefabs[Random.Range(0, grassPrefabs.Count)];
                            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                            instance.transform.position = hit.point;
                            instance.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                            instance.transform.SetParent(currentGroup.transform);
                            instance.name = "[Spawned_Grass]";
                            spawnedGrass.Add(instance);
                        }
                    }
                }
            }
        }

        Debug.Log("Grass spawned into group.");
    }

    private void ClearGrass()
    {
        foreach (var grass in spawnedGrass)
        {
            if (grass != null)
                DestroyImmediate(grass);
        }
        spawnedGrass.Clear();

        if (currentGroup != null)
            DestroyImmediate(currentGroup);

        Debug.Log("All spawned grass cleared.");
    }

    private void ClearFromParentOnly()
    {
        if (parentObject == null)
        {
            Debug.LogWarning("Parent Object not assigned.");
            return;
        }

        int count = 0;
        foreach (Transform child in parentObject.transform)
        {
            if (child.name == groupName)
            {
                DestroyImmediate(child.gameObject);
                count++;
            }
        }

        Debug.Log($"Cleared {count} grass groups from parent.");
    }

    private void SaveAsPrefab()
    {
        if (currentGroup == null)
        {
            Debug.LogWarning("No group to save. Spawn something first.");
            return;
        }

        PrefabUtility.SaveAsPrefabAssetAndConnect(currentGroup, prefabSavePath, InteractionMode.UserAction);
        Debug.Log("Saved prefab to: " + prefabSavePath);
    }
}
