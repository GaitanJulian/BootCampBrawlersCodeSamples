using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTree : MonoBehaviour
{
    [SerializeField] private float initialSpeed = 50.0f;
    [SerializeField] private float speed;
    public void SetInitialSpeed(float speedIncrease)
    {
        speed = initialSpeed + speedIncrease * 1.75f;
    }
    private void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed * 25 * -1);
    }
}
