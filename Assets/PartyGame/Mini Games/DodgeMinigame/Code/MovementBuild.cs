using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBuild : MonoBehaviour
{
    [SerializeField] private float initialSpeed = 5.0f;
    [SerializeField] private float speed;
    [SerializeField] private float direction = -1f;
    public void SetInitialSpeed(float speedIncrease)
    {
        speed = initialSpeed + speedIncrease * 1.75f;
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed * direction);
    }
}
