using Cinemachine;
using MenteBacata.ScivoloCharacterControllerDemo;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VictoryCircuite3D : MonoBehaviour
{
    public GameObject[] waypoints;
    public float playerSpeed = 2f;
    public int waypointIndex = 0;
    GameObject jugador;
    public GameObject StartWin;

    [SerializeField] GameObject fx;

    private void Start()
    {
        fx.SetActive(false);
    }
    private void FixedUpdate()
    {

        //if (jugador != null) { Move(); }

    }

    private void Move()
    {
        if (Vector3.Distance(jugador.transform.position, waypoints[waypointIndex].transform.position) < 0.1f)
        {
            waypointIndex++;
            if (waypointIndex >= waypoints.Length)
            {
                waypointIndex = 0;
            }
        }
        jugador.transform.position = Vector3.MoveTowards(transform.position, waypoints[waypointIndex].transform.position, playerSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        {
            jugador = other.gameObject;

            SimpleCharacterController playerController = jugador.GetComponent<SimpleCharacterController>();
            CinemachineFreeLook camera = jugador.GetComponent<CinemachineFreeLook>();

            fx.SetActive(true);
            //GetComponent<Collider>().enabled = false;
            if (playerController != null)
            {
                //playerController.enabled = false;
                //camera.enabled = false;

            }
        }
    }
}
