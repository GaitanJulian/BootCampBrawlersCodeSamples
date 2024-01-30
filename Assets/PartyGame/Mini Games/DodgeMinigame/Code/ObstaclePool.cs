using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePool : MonoBehaviour
{
    public GameObject[] ObstaclesPrefabs;
    public int poolSize = 20;

    private List<GameObject> pool;

    private void Start()
    {
        pool = new List<GameObject>(poolSize);
        for (int i = 0; i < poolSize; i++)
        {
            int obstacleIndex = Random.Range(0, ObstaclesPrefabs.Length);
            GameObject obstacle = Instantiate(ObstaclesPrefabs[obstacleIndex], this.transform);
            obstacle.SetActive(false);
            pool.Add(obstacle);
        }
    }

    public GameObject GetFromPool()
    {
        foreach (GameObject obstacle in pool)
        {
            if (!obstacle.activeInHierarchy)
            {
                obstacle.SetActive(true);
                return obstacle;
            }
        }
        int obstacleIndex = Random.Range(0, ObstaclesPrefabs.Length);
        GameObject newObstacle = Instantiate(ObstaclesPrefabs[obstacleIndex], this.transform);
        newObstacle.SetActive(true);
        pool.Add(newObstacle);
        return newObstacle;
    }

    public void ReturnToPool(GameObject obstacle)
    {
        obstacle.SetActive(false);
    }
}
