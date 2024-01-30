using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class FastLaneMovement : MonoBehaviour
{
    private InputActionAsset inputActions;
    private InputActionMap player;
    private InputAction move;
    //private InputAction turbo;

    [SerializeField] private float speedTurn;
    [SerializeField] private float moveSpeed;

    [SerializeField] private float tiltShip;

    [SerializeField] private float playerLane;

    private bool turboOn = false;
 
    Rigidbody rb;

    private bool isInitialized = false;

    public void StartMovementBehavior()
    {
        inputActions = GetComponentInChildren<PlayerInput>().actions;
        player = inputActions.FindActionMap("Player");
        rb = GetComponent<Rigidbody>();

        move = player.FindAction("Movement");
        //turbo = player.FindAction("Run");
        player.Enable();
        isInitialized = true;
    }

    public void StopMovementBehavior()
    {
        player.Disable();
    }

    
    private void Update()
    {
        if (isInitialized)
        {
            Vector3 directionMove = new Vector3(0, 0, move.ReadValue<Vector2>().y);

            Vector3 rotation = new Vector3(0, move.ReadValue<Vector2>().x, 0);

            if (directionMove != Vector3.zero)
            {
                rb.AddForce(transform.forward * moveSpeed * Time.deltaTime, ForceMode.Impulse);
                //TODO:AUDIOMOVIMIENTO

            }

            if (rotation != Vector3.zero)
            {
                rb.maxAngularVelocity = rotation.magnitude * speedTurn * 1.2f * Time.deltaTime;
                rb.angularVelocity = rotation * speedTurn;

            }


            if (turboOn)
            {
                rb.AddForce(transform.forward * moveSpeed * 1.1f * Time.deltaTime, ForceMode.Impulse);
                rb.angularVelocity = rotation * speedTurn * 1.1f * Time.deltaTime;
                StartCoroutine(DeactivateTurbo());
                //TODO:SONIDO TURBO
            }
        }
        
    }

    private IEnumerator DeactivateTurbo()
    {
        yield return new WaitForSeconds(1.5f); // Duración del turbo (ajusta según tus necesidades).

        // Desactiva el turbo.
        turboOn = false;
    }



    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Root")
        {
            Vector3 position = transform.position;

            // set the coordinates to spawn when comin back to the lane.

            transform.DOMove(position - new Vector3(0, 4, 0), 3).OnComplete(MoveToSpawn);
        }
    }

    private void MoveToSpawn()
    {

        Vector3 position = transform.position + Vector3.up * 4;

        // set the coordinates to spawn when comin back to the lane.

        Vector3 spawnPos = position.normalized * position.magnitude * playerLane * 0.3f;

        transform.position = new Vector3(spawnPos.x, 0.17f, spawnPos.z);

        transform.forward = Cross(Vector3.up, position);
    }

    Vector3 Cross(Vector3 v, Vector3 w)
    {
        float xValue = v.y * w.z - v.z * w.y;
        float yvalue = v.x * w.z - w.x * v.z;
        float zvalue = v.x * w.y - w.x * v.y;

        return new Vector3(xValue, yvalue, zvalue);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Fuel")
        {
            turboOn = true;
        }
    }
}
