// ObstacleSpawner.cs - Spawns obstacles dynamically along the track

using UnityEngine;
using System.Collections.Generic;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] obstaclePrefabs;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float spawnDistance = 50f;

    private ObjectPool obstaclePool;
    private List<GameObject> activeObstacles = new List<GameObject>();
    private float nextSpawnZ = 20f;
    private float spawnInterval = 3f;
    private float currentSpeedMultiplier = 1f;

    private void Start()
    {
        playerTransform = FindObjectOfType<PlayerController>().transform;
        obstaclePool = GetComponent<ObjectPool>();
        if (obstaclePool == null)
            obstaclePool = gameObject.AddComponent<ObjectPool>();
    }

    private void Update()
    {
        if (playerTransform == null) return;

        // Spawn obstacles ahead
        while (nextSpawnZ < playerTransform.position.z + spawnDistance)
        {
            SpawnObstacle();
            nextSpawnZ += spawnInterval / currentSpeedMultiplier;
        }

        // Destroy obstacles behind player
        for (int i = activeObstacles.Count - 1; i >= 0; i--)
        {
            if (activeObstacles[i].transform.position.z < playerTransform.position.z - 20f)
            {
                obstaclePool.ReturnObject(activeObstacles[i]);
                activeObstacles.RemoveAt(i);
            }
        }
    }

    private void SpawnObstacle()
    {
        if (obstaclePrefabs.Length == 0) return;

        // Random obstacle type
        GameObject prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
        GameObject obstacle = obstaclePool.GetObject();
        
        // Random lane
        int lane = Random.Range(0, GameConstants.NUM_LANES);
        float laneOffset = (lane - 1) * GameConstants.LANE_WIDTH;
        
        obstacle.transform.position = new Vector3(laneOffset, 1, nextSpawnZ);
        obstacle.SetActive(true);
        activeObstacles.Add(obstacle);
    }

    public void SetDifficultyMultiplier(float multiplier)
    {
        currentSpeedMultiplier = multiplier;
        spawnInterval = Mathf.Max(0.5f, GameConstants.OBSTACLE_MAX_SPAWN_INTERVAL / multiplier);
    }
}
