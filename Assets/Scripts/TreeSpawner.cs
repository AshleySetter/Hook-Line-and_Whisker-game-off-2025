using UnityEngine;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class treesSpawner : MonoBehaviour
{
    [Header("References")]
    public Tilemap treesTilemap;
    public GameObject[] treesPrefabs;

    [Header("Settings")]
    [Range(0f, 1f)]
    public float spawnChance = 0.8f;

    [Header("Randomization")]
    public Vector2 randomOffsetRange = new Vector2(0.3f, 0.3f);
    public Vector2 randomScaleRange = new Vector2(0.8f, 1.2f);

#if UNITY_EDITOR
    [ContextMenu("Spawn trees Now")]
    public void Spawntrees()
    {
        if (treesTilemap == null || treesPrefabs == null || treesPrefabs.Length == 0)
        {
            Debug.LogError("Missing references or no trees prefabs assigned!");
            return;
        }

        Undo.IncrementCurrentGroup();
        Undo.SetCurrentGroupName("Spawn trees");

        // Create a parent container for all spawned trees
        GameObject container = new GameObject("treesContainer");
        container.transform.SetParent(transform);

        foreach (var pos in treesTilemap.cellBounds.allPositionsWithin)
        {
            if (!treesTilemap.HasTile(pos))
                continue;

            if (Random.value > spawnChance)
                continue;

            // Pick random prefab
            GameObject prefab = treesPrefabs[Random.Range(0, treesPrefabs.Length)];

            // Calculate world position
            Vector3 worldPos = treesTilemap.CellToWorld(pos) + treesTilemap.tileAnchor;
            worldPos += new Vector3(
                Random.Range(-randomOffsetRange.x, randomOffsetRange.x),
                Random.Range(-randomOffsetRange.y, randomOffsetRange.y),
                0f
            );

            // Instantiate using editor-safe method so objects persist
            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab, container.transform);
            instance.transform.position = worldPos;

            float scale = Random.Range(randomScaleRange.x, randomScaleRange.y);
            instance.transform.localScale = new Vector3(scale, scale, 1f);
            // instance.transform.Rotate(0, 0, Random.Range(0f, 360f));

            Undo.RegisterCreatedObjectUndo(instance, "Spawn trees");
        }

        Undo.RegisterCreatedObjectUndo(container, "Spawn trees");
        Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
    }

    [ContextMenu("Clear Spawned trees")]
    public void Cleartrees()
    {
        Transform existing = transform.Find("treesContainer");
        if (existing != null)
        {
            Undo.DestroyObjectImmediate(existing.gameObject);
        }
    }
#endif
}
