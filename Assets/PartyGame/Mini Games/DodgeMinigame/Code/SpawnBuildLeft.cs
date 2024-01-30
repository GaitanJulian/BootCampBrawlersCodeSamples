using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBuildLeft : MonoBehaviour
{
    public BuildPoolLeft buildPoolLeft;
    [SerializeField] private float spawnRangeX = 10;
    [SerializeField] private float spawnRangeZ = 10;
    [SerializeField] private float startDelay = 5;
    [SerializeField] private float spawnInterval = 1.0f;
    [SerializeField] private float spawnIntervalIncrease = 0.001f;
    [SerializeField] private float speedIncrease = 0.1f;

    private void Start()
    {
        StartCoroutine(SpawnRandomBuildLeftWithDelay(1f));
    }

    private IEnumerator SpawnRandomBuildLeftWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(SpawnRandomBuildLeft());
    }

    private IEnumerator SpawnRandomBuildLeft()
    {
        while (true)
        {
            if (buildPoolLeft != null) // Verifica que buildPoolLeft no sea null
            {
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), 0, spawnRangeZ);

                GameObject build = buildPoolLeft.GetFromPoolBuildLeft();
                if (build != null) // Verifica que build no sea null
                {
                    build.transform.position = spawnPosition;

                    MovementBuild movement = build.GetComponent<MovementBuild>();
                    if (movement != null)
                    {
                        speedIncrease = speedIncrease + 0.05f;
                        movement.SetInitialSpeed(speedIncrease);
                    }
                    yield return new WaitForSeconds(spawnInterval);
                    spawnInterval -= 0.00025f;
                    spawnIntervalIncrease += 0.1f;
                }
                else
                {
                    Debug.LogWarning("El objeto obtenido de la piscina es nulo.");
                }
            }
            else
            {
                Debug.LogWarning("La piscina de objetos es nula.");
            }
        }
    }
}
