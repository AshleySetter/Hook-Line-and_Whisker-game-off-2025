using UnityEngine;

public class CatSpawnScheduler : MonoBehaviour
{
    [SerializeField] private float timeBetweenSpawns;
    private float spawnTimer;

    // Update is called once per frame
    void Update()
    {
        if (spawnTimer > timeBetweenSpawns)
        {
            CatSpawnPoint.SpawnRandomCat();
            spawnTimer = 0;
        }
        spawnTimer += Time.deltaTime;
    }
}
