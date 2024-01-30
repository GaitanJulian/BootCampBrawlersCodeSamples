using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelTurnOff : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);
        FuelPooler.SharedInstance.ReturnFuel(gameObject);
    }
}
