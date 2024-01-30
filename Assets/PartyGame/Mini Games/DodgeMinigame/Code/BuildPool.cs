using System.Collections.Generic;
using UnityEngine;

public class BuildPool : MonoBehaviour
{
    public GameObject[] buildsPrefabs;
    public int poolSize = 20;
 
    private List<GameObject> poolBuilds;

    private void Start()
    {
        poolBuilds = new List<GameObject>(poolSize);
        for (int i = 0; i < poolSize; i++)
        {
            int buildIndex = Random.Range(0, buildsPrefabs.Length);
            GameObject build = Instantiate(buildsPrefabs[buildIndex], this.transform);
            build.SetActive(false);
            poolBuilds.Add(build);
        }
    }

    public GameObject GetFromPoolBuild()
    {
        foreach (GameObject build in poolBuilds)
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
        poolBuilds.Add(newBuild);
        return newBuild;
    }

    public void ReturnToPool(GameObject build)
    {
        build.SetActive(false);
    }
}
