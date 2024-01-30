using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObstacles : MonoBehaviour
{
    public ObstaclePool obstaclePool;
    [SerializeField] private float spawnRangeX = 10;
    [SerializeField] private float spawnRangeZ = 10;
    [SerializeField] private float startDelay = 5; 
    [SerializeField] private float spawnInterval = 1.0f; 
    [SerializeField] private float spawnIntervalIncrease = 0.001f; 
    [SerializeField] private float speedIncrease = 0.1f; 

    private void Start()
    {
        StartCoroutine(SpawnRandomObstacle());
    }

    private IEnumerator SpawnRandomObstacle()
    {
        while (true)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), 0, spawnRangeZ);

            GameObject obstacle = obstaclePool.GetFromPool();
            obstacle.transform.position = spawnPosition;

            MovementObstacle movement = obstacle.GetComponent<MovementObstacle>();
            if (movement != null)
            {
                speedIncrease = speedIncrease + 0.05f;
                movement.SetInitialSpeed(speedIncrease);
            }

            yield return new WaitForSeconds(spawnInterval);

            spawnInterval -= 0.00025f;
            spawnIntervalIncrease += 0.1f;
        }
    }
}
