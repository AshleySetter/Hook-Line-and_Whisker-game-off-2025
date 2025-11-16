using UnityEngine;

public class CatSpawnScheduler : MonoBehaviour
{
    [SerializeField] private float timeBetweenSpawns;
    [SerializeField] private int maxCats;
    private float spawnTimer;
    private int catsSpawned;

    private void Start()
    {
        catsSpawned = 0;
    }

    private void Update()
    {
        if (spawnTimer > timeBetweenSpawns && catsSpawned < maxCats)
        {
            CatSpawnPoint.SpawnRandomCat();
            spawnTimer = 0;
            catsSpawned++;
        }
        spawnTimer += Time.deltaTime;
    }
}
