using UnityEngine;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class GrassSpawner : MonoBehaviour
{
    [Header("References")]
    public Tilemap grassTilemap;
    public GameObject[] grassPrefabs;

    [Header("Settings")]
    [Range(0f, 1f)]
    public float spawnChance = 0.8f;

    [Header("Randomization")]
    public Vector2 randomOffsetRange = new Vector2(0.3f, 0.3f);
    public Vector2 randomScaleRange = new Vector2(0.8f, 1.2f);

#if UNITY_EDITOR
    [ContextMenu("Spawn Grass Now")]
    public void SpawnGrass()
    {
        if (grassTilemap == null || grassPrefabs == null || grassPrefabs.Length == 0)
        {
            Debug.LogError("Missing references or no grass prefabs assigned!");
            return;
        }

        Undo.IncrementCurrentGroup();
        Undo.SetCurrentGroupName("Spawn Grass");

        // Create a parent container for all spawned grass
        GameObject container = new GameObject("GrassContainer");
        container.transform.SetParent(transform);

        foreach (var pos in grassTilemap.cellBounds.allPositionsWithin)
        {
            if (!grassTilemap.HasTile(pos))
                continue;

            if (Random.value > spawnChance)
                continue;

            // Pick random prefab
            GameObject prefab = grassPrefabs[Random.Range(0, grassPrefabs.Length)];

            // Calculate world position
            Vector3 worldPos = grassTilemap.CellToWorld(pos) + grassTilemap.tileAnchor;
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

            Undo.RegisterCreatedObjectUndo(instance, "Spawn Grass");
        }

        Undo.RegisterCreatedObjectUndo(container, "Spawn Grass");
        Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
    }

    [ContextMenu("Clear Spawned Grass")]
    public void ClearGrass()
    {
        Transform existing = transform.Find("GrassContainer");
        if (existing != null)
        {
            Undo.DestroyObjectImmediate(existing.gameObject);
        }
    }
#endif
}
