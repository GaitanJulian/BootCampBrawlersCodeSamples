using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml.Schema;
using UnityEngine;

public class FuelGenerator : MonoBehaviour
{
    private float elapsedTime = 0;
    private float duration = 2;

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= duration)
        {
            NeedFuel();
            elapsedTime = 0f; // Reinicia el temporizador
            duration = Random.Range(2f, 4.7f); // Establece una nueva duración aleatoria
        }
    }

    public void NeedFuel()
    {
        GameObject fuel = FuelPooler.SharedInstance.GetSomeFuel();
        if (fuel != null) 
        {
            fuel.transform.position = GenVect();
            fuel.SetActive(true);
        }
    }

    Vector3 GenVect()
    {
        float r1 = 11;
        float r2 = 25;

        float randomValue = Random.value;

        // Interpola para obtener una distancia radial en el rango [r1, r2]
        float radialDistance = Mathf.Lerp(r1, r2, randomValue);

        // Genera un ángulo aleatorio en el rango [0, 360] grados
        float randomAngle = Random.Range(0f, 360f);

        // Convierte el ángulo a radianes
        float angleInRadians = Mathf.Deg2Rad * randomAngle;

        float xValue = radialDistance * Mathf.Sin(angleInRadians);  
        float yValue = 0.17f;
        float zValue = radialDistance * Mathf.Cos(angleInRadians);

        return new Vector3(xValue, yValue, zValue);
    }
}
