using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelPooler : MonoBehaviour
{
    public static FuelPooler SharedInstance;

    public GameObject objectToPool;
    [SerializeField] public int amountToPool = 3;

    public Queue<GameObject> fuelPool = new Queue<GameObject>();

    public bool canCreate = true;

    private void Awake()
    {
        SharedInstance = this;
        FillPool(amountToPool);
    }

    private void FillPool(int size)
    {
        for (int i = 0; i < size; i++)
        {
            CreateFuel();
        }
    }

    public void ReturnFuel(GameObject fuel)
    {
        fuel.transform.position = Vector3.zero;
        fuel.SetActive(false);

        fuelPool.Enqueue(fuel);
    }

    public GameObject GetSomeFuel()
    {
        if(fuelPool.Count == 0)
        {
            return null;
        }
        GameObject fuel = fuelPool.Dequeue();

        return fuel;
    }

    public void CreateFuel()
    {
        GameObject objFuel = Instantiate(objectToPool, transform);
        fuelPool.Enqueue(objFuel);
        objFuel.SetActive(false);
    } 
}
