using UnityEngine;
using UnityEditor;

public class ColliderSpriteGenerator : EditorWindow
{
    [SerializeField] private GameObject squarePrefab;
    [SerializeField] private GameObject circlePrefab;
    [SerializeField] private GameObject capsulePrefab;

    [MenuItem("Tools/Generate Collider Sprites")]
    static void OpenWindow()
    {
        GetWindow<ColliderSpriteGenerator>("Collider Sprite Generator");
    }

    void OnGUI()
    {
        squarePrefab = (GameObject)EditorGUILayout.ObjectField("Square Prefab", squarePrefab, typeof(GameObject), false);
        circlePrefab = (GameObject)EditorGUILayout.ObjectField("Circle Prefab", circlePrefab, typeof(GameObject), false);
        capsulePrefab = (GameObject)EditorGUILayout.ObjectField("Capsule Prefab", capsulePrefab, typeof(GameObject), false);

        if (GUILayout.Button("Generate on Selected"))
        {
            foreach (var obj in Selection.objects)
            {
                Process(obj);
            }
        }
    }

    void Process(Object obj)
    {
        string path = AssetDatabase.GetAssetPath(obj);

        // If object is a prefab asset
        if (!string.IsNullOrEmpty(path) && obj is GameObject)
        {
            GameObject prefabRoot = PrefabUtility.LoadPrefabContents(path);
            GenerateForObject(prefabRoot);
            PrefabUtility.SaveAsPrefabAsset(prefabRoot, path);
            PrefabUtility.UnloadPrefabContents(prefabRoot);

            Debug.Log($"Processed prefab: {obj.name}");
        }
        else if (obj is GameObject go)
        {
            // Scene object
            GenerateForObject(go);
            Debug.Log($"Processed scene object: {obj.name}");
        }
        else
        {
            Debug.LogWarning("Selection is not a GameObject or prefab.");
        }
    }

    void GenerateForObject(GameObject root)
    {
        foreach (var col in root.GetComponentsInChildren<Collider2D>(true))
        {
            CreateSpriteChild(col);
        }
    }

    void CreateSpriteChild(Collider2D col)
    {
        GameObject prefab = null;
        Vector3 scale = Vector3.one;

        if (col is BoxCollider2D box)
        {
            prefab = squarePrefab;
            scale = box.size;
        }
        else if (col is CircleCollider2D circle)
        {
            float d = circle.radius * 2f;
            prefab = circlePrefab;
            scale = new Vector3(d, d, 1f);
        }
        else if (col is CapsuleCollider2D cap)
        {
            prefab = capsulePrefab;
            scale = new Vector3(cap.size.x, cap.size.y, 1f);
        }

        if (prefab == null)
            return;

        // Instantiate with NO parent first (Unity requirement)
        GameObject child = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

        // THEN parent it
        child.transform.SetParent(col.transform);

        child.name = col.name + "_NavSprite";
        child.transform.localPosition = col.offset;
        child.transform.localScale = scale;
    }
}
