using System.Collections.Generic;
using UnityEngine;

public class CatSpawnScheduler : MonoBehaviour
{
    public static CatSpawnScheduler Instance { get; private set; }

    private float spawnTimer;
    private int catsSpawned;
    private bool spawningStarted;
    private bool spawningFinished;
    private List<float> catSpawnTimes;

    private void Awake()
    {
        Instance = this;
        catSpawnTimes = new List<float>();
    }

    public void GenerateSpawnSchedule(int numberOfCatsToSpawn)
    {
        catSpawnTimes.Clear();
        float maxSpawnTime = 0.8f * DayNightTimer.Instance.GetSecondsInADay();
        for (int i = 0; i < numberOfCatsToSpawn; i++)
        {
            catSpawnTimes.Add(Random.Range(0, maxSpawnTime));
        }
    }

    public void StartSpawning()
    {
        spawningStarted = true;
        spawningFinished = false;
        catsSpawned = 0;   
    }

    private void Update()
    {
        if (spawningStarted && !spawningFinished) {
            for (int i = catsSpawned; i < catSpawnTimes.Count; i++ )
            {
                if (catSpawnTimes[i] > spawnTimer)
                {
                    CatSpawnPoint.SpawnRandomCat();
                    catsSpawned++;
                }
            }
            if (catsSpawned >= catSpawnTimes.Count) {
                spawningFinished = true;
            }
            spawnTimer += Time.deltaTime;
        }
    }
}
