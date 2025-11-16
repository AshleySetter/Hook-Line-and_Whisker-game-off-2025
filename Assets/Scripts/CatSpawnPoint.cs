using System;
using UnityEngine;

public class CatSpawnPoint : MonoBehaviour
{
    [SerializeField] private GameObject catPrefab;

    public void SpawnCat()
    {
        Instantiate(catPrefab, transform.position, Quaternion.identity);
    }

    // Static method to spawn a cat at a random spawn point
    public static void SpawnRandomCat()
    {
        // Find all CatSpawnPoint instances in the scene
        CatSpawnPoint[] spawnPoints = FindObjectsByType<CatSpawnPoint>(FindObjectsSortMode.None);

        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning("No CatSpawnPoints found in the scene!");
            return;
        }

        // Pick a random spawn point
        int index = UnityEngine.Random.Range(0, spawnPoints.Length);
        CatSpawnPoint chosenSpawn = spawnPoints[index];

        // Spawn the cat
        chosenSpawn.SpawnCat();
    }
}
