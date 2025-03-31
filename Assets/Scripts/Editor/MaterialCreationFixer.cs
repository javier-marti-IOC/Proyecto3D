using UnityEditor;
using UnityEngine;

public class MaterialCreationFixer : EditorWindow
{
    [MenuItem("Tools/Fix Material Creation Mode")]
    public static void ShowWindow()
    {
        string[] guids = AssetDatabase.FindAssets("t:Model"); // Busca todos los modelos del proyecto

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            ModelImporter importer = AssetImporter.GetAtPath(path) as ModelImporter;

            if (importer != null)
            {
                // Cambiar el modo de creación de materiales a Standard
                importer.materialImportMode = ModelImporterMaterialImportMode.ImportStandard;
                importer.SaveAndReimport();
            }
        }

        Debug.Log("✅ Todos los modelos han sido configurados con Material Creation Mode = Standard");
    }
}
