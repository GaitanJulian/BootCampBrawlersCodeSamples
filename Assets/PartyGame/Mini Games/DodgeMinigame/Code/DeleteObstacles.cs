using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteObstacles : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        other.gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.SetActive(false);
    }
}
