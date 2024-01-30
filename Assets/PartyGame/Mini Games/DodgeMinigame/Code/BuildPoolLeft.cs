using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPoolLeft : MonoBehaviour
{
    public GameObject[] buildsPrefabs;
    public int poolSize = 20;

    private List<GameObject> poolBuildsLeft;

    private void Start()
    {
        poolBuildsLeft = new List<GameObject>(poolSize);
        for (int i = 0; i < poolSize; i++)
        {
            int buildIndex = Random.Range(0, buildsPrefabs.Length);
            GameObject build = Instantiate(buildsPrefabs[buildIndex], this.transform);
            build.SetActive(false);
            poolBuildsLeft.Add(build);
        }
    }

    public GameObject GetFromPoolBuildLeft()
    {
        foreach (GameObject build in poolBuildsLeft)
        {
            if (!build.activeInHierarchy)
            {
                build.SetActive(true);
                return build;
            }
        }
        int buildIndex = Random.Range(0, buildsPrefabs.Length);
        GameObject newBuild = Instantiate(buildsPrefabs[buildIndex], this.transform);
        newBuild.SetActive(true);
        poolBuildsLeft.Add(newBuild);
        return newBuild;
    }

    public void ReturnToPool(GameObject build)
    {
        build.SetActive(false);
    }
}
