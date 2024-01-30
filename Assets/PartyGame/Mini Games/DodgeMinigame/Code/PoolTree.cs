using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolTree : MonoBehaviour
{
    public GameObject[] treePrefabs;
    public int poolSize = 20;

    private List<GameObject> poolTree;

    private void Start()
    {
        poolTree = new List<GameObject>(poolSize);
        for (int i = 0; i < poolSize; i++)
        {
            int treeIndex = Random.Range(0, treePrefabs.Length);
            GameObject tree = Instantiate(treePrefabs[treeIndex], this.transform);
            tree.SetActive(false);
            poolTree.Add(tree);
        }
    }

    public GameObject GetFromPoolTree()
    {
        foreach (GameObject tree in poolTree)
        {
            if (!tree.activeInHierarchy)
            {
                tree.SetActive(true);
                return tree;
            }
        }
        int treeIndex = Random.Range(0, treePrefabs.Length);
        GameObject newTree = Instantiate(treePrefabs[treeIndex], this.transform);
        newTree.SetActive(true);
        poolTree.Add(newTree);
        return newTree;
    }

    public void ReturnToPool(GameObject build)
    {
        build.SetActive(false);
    }
}
